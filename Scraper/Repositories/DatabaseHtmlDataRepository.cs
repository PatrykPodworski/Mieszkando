using MarklogicDataLayer;
using MarklogicDataLayer.DatabaseConnectors;
using MarklogicDataLayer.DataStructs;
using MarklogicDataLayer.XQuery;
using MarklogicDataLayer.XQuery.Functions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace OfferScraper.Repositories
{
    public class DatabaseHtmlDataRepository : IDataRepository<HtmlData>
    {
        private readonly IDatabaseConnectionSettings _databaseConnectionSettings;
        private readonly RestConnector _restConnector;

        public DatabaseHtmlDataRepository(IDatabaseConnectionSettings databaseConnectionSettings)
        {
            _databaseConnectionSettings = databaseConnectionSettings;
            _restConnector = new RestConnector(_databaseConnectionSettings);
        }

        public void Delete(HtmlData entity)
        {
            var offerType = entity.OfferType == OfferType.Olx ? "OlxHtmlData" : "OtoDomHtmlData";
            var query = new XdmpDocumentDelete(new MlUri($"{offerType}_{entity.Id}", MlUriDocumentType.Xml)).Query;
            _restConnector.Submit(query);
        }

        public IQueryable<HtmlData> GetAll()
        {
            var query = new CtsSearch("/", new CtsCollectionQuery(new[] { "OlxHtmlData", "OtoDomHtmlData" })).Query;
            return GetFromQuery(query);
        }

        public HtmlData GetById(int id)
        {
            var query = new CtsSearch("/", new CtsElementValueQuery("html_data_id", id.ToString())).Query;
            var response = _restConnector.Submit(query);

            if (!response.Content.IsMimeMultipartContent())
                return null;

            var content = response.Content.ReadAsMultipartAsync().Result.Contents;
            foreach (var data in content)
            {
                var text = ReadAsString(data);
                var xml = XDocument.Parse(text);
                var htmlDataId = xml.Descendants().Where(x => x.Name == "html_data_id").First().Value;
                if (htmlDataId == id.ToString())
                {
                    return ExtractHtmlDataInfo(xml, htmlDataId);
                }
            }

            return null;
        }

        public void Insert(HtmlData entity)
        {
            var offerType = entity.OfferType == OfferType.Olx ? "OlxHtmlData" : "OtoDomHtmlData";
            using (var writer = new StringWriter())
            using (var xmlWriter = XmlWriter.Create(writer))
            {
                new XmlSerializer(entity.GetType()).Serialize(writer, entity);
                var serializedHtmlData = writer.GetStringBuilder().ToString();
                var content = MarklogicContent.Xml($"{offerType}_{entity.Id}", serializedHtmlData, new[] { offerType });
                _restConnector.Insert(content);
            }
        }

        public IQueryable<HtmlData> SearchFor(Expression<Func<HtmlData, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public void Insert(HtmlData entity, MlTransactionScope transaction)
        {
            var offerType = entity.OfferType == OfferType.Olx ? "OlxHtmlData" : "OtoDomHtmlData";
            using (var writer = new StringWriter())
            using (var xmlWriter = XmlWriter.Create(writer))
            {
                new XmlSerializer(entity.GetType()).Serialize(writer, entity);
                var serializedHtmlData = writer.GetStringBuilder().ToString();
                var content = MarklogicContent.Xml($"{offerType}_{entity.Id}", serializedHtmlData, new[] { offerType });
                _restConnector.Insert(content, transaction);
            }
        }

        public void Insert(IEnumerable<HtmlData> entities, MlTransactionScope transaction)
        {
            foreach (var entity in entities)
            {
                Insert(entity, transaction);
            }
        }

        public void Update(HtmlData entity) => Insert(entity);

        public void Update(HtmlData entity, MlTransactionScope transaction) => Insert(entity, transaction);

        public void Update(IEnumerable<HtmlData> entities, MlTransactionScope transaction) => Insert(entities, transaction);

        public void Insert(IEnumerable<HtmlData> entities)
        {
            var transaction = _restConnector.BeginTransaction();
            foreach (var entity in entities)
            {
                Insert(entity, transaction);
            }
            _restConnector.CommitTransaction(transaction);
        }

        public void Update(IEnumerable<HtmlData> entities) => Insert(entities);

        public IQueryable<HtmlData> Get(MarklogicDataLayer.XQuery.Expression expression, long numberOfElements = long.MinValue)
        {
            var query = new FnSubsequence(new CtsSearch("/", expression), numberOfElements).Query;
            return GetFromQuery(query);
        }

        private static string ReadAsString(HttpContent content)
        {
            return content.ReadAsStringAsync().Result;
        }

        private static HtmlData ExtractHtmlDataInfo(XDocument xml, string htmlDataId)
        {
            var htmlDataOfferContent = xml.Descendants().Where(x => x.Name == "offer_content").First().Value;
            var htmlDataOfferType = xml.Descendants().Where(x => x.Name == "offer_type").First().Value == "Olx" ? OfferType.Olx : OfferType.OtoDom;
            var htmlDataLastUpdate = DateTime.Parse(xml.Descendants().Where(x => x.Name == "last_update").First().Value);
            var htmlDataStatus = Status.New;
            switch (xml.Descendants().Where(x => x.Name == "status").First().Value)
            {
                case "New":
                    htmlDataStatus = Status.New;
                    break;

                case "InProcess":
                    htmlDataStatus = Status.InProgress;
                    break;

                case "Success":
                    htmlDataStatus = Status.Success;
                    break;

                case "Fatal":
                    htmlDataStatus = Status.Failed;
                    break;
            }
            return new HtmlData()
            {
                Id = htmlDataId,
                Status = htmlDataStatus,
                LastUpdate = htmlDataLastUpdate,
                OfferType = htmlDataOfferType,
                Content = htmlDataOfferContent,
            };
        }

        private IQueryable<HtmlData> GetFromQuery(string query)
        {
            var response = _restConnector.Submit(query);

            if (!response.Content.IsMimeMultipartContent())
                return new List<HtmlData>().AsQueryable();

            var content = response.Content.ReadAsMultipartAsync().Result.Contents;
            var result = new List<HtmlData>();
            foreach (var data in content)
            {
                var text = ReadAsString(data);
                var xml = XDocument.Parse(text);
                var htmlDataId = xml.Descendants().Where(x => x.Name == "html_data_id").First().Value;
                result.Add(ExtractHtmlDataInfo(xml, htmlDataId));
            }
            return result.AsQueryable();
        }
        public ITransaction GetTransaction()
        {
            return new DatabaseTransaction(_restConnector);
        }
    }
}