using MarklogicDataLayer.DatabaseConnectors;
using MarklogicDataLayer.XQuery;
using MarklogicDataLayer.XQuery.Functions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security;
using System.Text;

namespace MarklogicDataLayer
{
    public interface IRestConnector
    {
        void Insert(MarklogicContent marklogicContent);
        void Insert(MarklogicContent marklogicContent, MlTransactionScope transaction);
        void Insert(MarklogicContent[] marklogicContent);
        void Insert(MarklogicContent[] marklogicContent, MlTransactionScope transaction);
        HttpResponseMessage Submit(string query);
        HttpResponseMessage Submit(string query, MlTransactionScope transaction);
    }

    public class RestConnector : IRestConnector
    {
        private readonly HttpClient _client;

        public RestConnector(IDatabaseConnectionSettings connectionSettings)
        {
            var clientHandler = new HttpClientHandler()
            {
                Credentials = new NetworkCredential(connectionSettings.UserName, connectionSettings.Password),
                PreAuthenticate = true,
                AllowAutoRedirect = false,
                UseCookies = false,
                MaxConnectionsPerServer = 1
            };

            _client = new HttpClient(clientHandler)
            {
                Timeout = TimeSpan.FromSeconds(60),
                BaseAddress = new Uri($"http://{connectionSettings.Host}:{connectionSettings.Port}")
            };
        }

        public void Insert(MarklogicContent marklogicContent)
        {
            Insert(new[] { marklogicContent });
        }

        public void Insert(MarklogicContent marklogicContent, MlTransactionScope transaction)
        {
            Insert(new[] { marklogicContent }, transaction);
        }

        public void Insert(MarklogicContent[] marklogicContent)
        {
            Insert(marklogicContent, null);
        }

        public void Insert(MarklogicContent[] marklogicContent, MlTransactionScope transaction)
        {
            foreach (var doc in marklogicContent)
            {
                var collections = doc.Collections?.Length > 0
                    ? $", ( {string.Join(", ", doc.Collections.Where(x => !string.IsNullOrEmpty(x)).Select(x => $"'{x}'"))} )"
                    : string.Empty;

                var content = "";
                switch (doc.Media)
                {
                    case MediaType.Xml:
                        content = $"xdmp:unquote('{EscapeXml(RemoveXmlHeader(doc.Content))}')";
                        break;
                    case MediaType.Json:
                        content = $"xdmp:unquote('{doc.Content}')";
                        break;
                    default:
                        throw new ArgumentException("Unknown mime type");
                }

                var query = "xdmp:document-insert(" +
                            $"'{doc.DocumentName}'," +
                            $"{content}, " +
                            "xdmp:default-permissions()" +
                            $"{collections})";

                var resp = transaction == null ? Submit(query) : Submit(query, transaction);
            }
        }

        public HttpResponseMessage Submit(string query)
        {
            var content = new StringContent(WebUtility.UrlEncode("xquery") + "=" + WebUtility.UrlEncode(query), null, "application/x-www-form-urlencoded");
            var result = Post(_client, "eval", content);

            if (!result.IsSuccessStatusCode)
                throw new HttpRequestException(result.Content.ReadAsStringAsync().Result);

            return result;
        }

        public HttpResponseMessage Submit(string query, MlTransactionScope transaction)
        {
            var content = new StringContent(WebUtility.UrlEncode("xquery") + "=" + WebUtility.UrlEncode(query), null, "application/x-www-form-urlencoded");
            var result = Post(_client, $"eval?txid={transaction.TransactionId}", content, transaction.Cookies);

            if (!result.IsSuccessStatusCode)
                throw new HttpRequestException(result.Content.ReadAsStringAsync().Result);

            transaction.Cookies = GetCookies(result);

            return result;
        }

        public MlTransactionScope BeginTransaction()
        {
            //https://docs.marklogic.com/REST/POST/v1/transactions
            var response = Post(_client, $"LATEST/transactions?timeLimit=60", new StringContent("", Encoding.UTF8));

            if (response.StatusCode != HttpStatusCode.SeeOther)
                throw new HttpRequestException(response.Content.ReadAsStringAsync().Result);

            var transaction = new MlTransactionScope
            {
                TransactionId = response.Headers.Location.OriginalString.Split('/').Last(),
                Cookies = GetCookies(response)
            };

            Submit($"xquery version '1.0-ml'; declare option xdmp:transaction-mode 'update'; xdmp:set-transaction-time-limit(60);", transaction);

            return transaction;
        }

        public void AcquireLocks(MlTransactionScope transaction, params string[] lockFiles)
        {
            var batchLock = new MultiStatement();

            foreach (var lockValue in lockFiles)
            {
                var query = new XdmpLockForUpdate(new MlUri(lockValue, MlUriDocumentType.Xml));
                batchLock.Add(query);
            }

            Submit(batchLock.Query, transaction);
        }

        public void CommitTransaction(MlTransactionScope transaction)
        {
            var result = Submit("xdmp:commit()", transaction);

            if (!result.IsSuccessStatusCode)
                throw new HttpRequestException(result.Content.ReadAsStringAsync().Result);
        }

        public void RollbackTransaction(MlTransactionScope transaction)
        {
            var result = Submit("xdmp:rollback()", transaction);

            if (!result.IsSuccessStatusCode)
                throw new HttpRequestException(result.Content.ReadAsStringAsync().Result);
        }

        private static HttpResponseMessage Post(HttpClient client, string method, HttpContent content, string cookies = "")
        {
            var message = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(client.BaseAddress + method),
                Content = content
            };

            message.Headers.Add("Cookie", string.IsNullOrEmpty(cookies) ? "TxnID=null" : cookies);

            return client.SendAsync(message).Result;
        }
        private static string RemoveXmlHeader(string content)
        {
            var start = content.IndexOf("<?", StringComparison.Ordinal);
            var end = content.IndexOf("?>", StringComparison.Ordinal);

            if (start >= 0 && start != end)
                return content.Remove(start, end - start + 2);
            return content;
        }

        //http://developer.marklogic.com/pipermail/general/2015-August/017790.html
        //https://msdn.microsoft.com/pl-pl/library/system.security.securityelement.escape(v=vs.110).aspx
        private static string EscapeXml(string content)
        {
            return SecurityElement.Escape(content);
        }

        private static string GetCookies(HttpResponseMessage response)
        {
            var c = new List<string>();

            if (response.Headers.TryGetValues("Set-Cookie", out var cookies))
            {
                c.AddRange(cookies.Select(cookie => cookie.Split(';')[0]));
            }
            else
            {
                return "";
            }

            return c.Aggregate((current, next) => $"{current}; {next}");
        }

        private static string FormatQuery(string query) => query.Replace("\r", " ").Replace("\n", " ");
    }
}
