using MarklogicDataLayer;
using MarklogicDataLayer.DatabaseConnectors;
using MarklogicDataLayer.DataStructs;
using MarklogicDataLayer.XQuery;
using MarklogicDataLayer.XQuery.Functions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace OfferScraper.Repositories
{
    public class DatabaseLinkRepository : IDataRepository<Link>
    {
        private readonly IDatabaseConnectionSettings _databaseConnectionSettings;
        private readonly RestConnector _restConnector;

        public DatabaseLinkRepository(IDatabaseConnectionSettings databaseConnectionSettings)
        {
            _databaseConnectionSettings = databaseConnectionSettings;
            _restConnector = new RestConnector(_databaseConnectionSettings);
        }

        public void Delete(Link entity)
        {
            var linkKind = entity.LinkSourceKind == OfferType.Olx ? "Olx" : "OtoDom";
            var query = new XdmpDocumentDelete(new MlUri($"{linkKind}_link_{entity.Id}", MlUriDocumentType.Xml)).Query;
            _restConnector.Submit(query);
        }

        public IQueryable<Link> GetAll()
        {
            var query = new CtsSearch("/", new CtsCollectionQuery(new[] { "Olx", "OtoDom" })).Query;
            return GetFromQuery(query);
        }

        public Link GetById(int id)
        {
            var query = new CtsSearch("/", new CtsElementValueQuery("link_id", id.ToString())).Query;
            var response = _restConnector.Submit(query);

            if (!response.Content.IsMimeMultipartContent())
                return null;

            var content = response.Content.ReadAsMultipartAsync().Result.Contents;
            foreach (var data in content)
            {
                var text = ReadAsString(data);
                var xml = XDocument.Parse(text);
                var linkId = xml.Descendants().Where(x => x.Name == "link_id").First().Value;
                if (linkId == id.ToString())
                {
                    return ExtractLinkInfo(xml, linkId);
                }
            }

            return null;
        }

        public void Insert(Link entity)
        {
            Insert(entity, null);
        }

        public void Insert(Link entity, MlTransactionScope transaction)
        {
            var linkKind = entity.LinkSourceKind == OfferType.Olx ? "Olx" : "OtoDom";
            using (var writer = new StringWriter())
            using (var xmlWriter = XmlWriter.Create(writer))
            {
                new XmlSerializer(entity.GetType()).Serialize(writer, entity);
                var serializedLink = writer.GetStringBuilder().ToString();
                var content = MarklogicContent.Xml($"{linkKind}_link_{entity.Id}", serializedLink, new[] { linkKind });
                _restConnector.Insert(content, transaction);
            }
        }

        public void Insert(IEnumerable<Link> entities, MlTransactionScope transaction)
        {
            foreach (var entity in entities)
            {
                Insert(entity, transaction);
            }
        }

        public void Insert(IEnumerable<Link> entities)
        {
            var transaction = _restConnector.BeginTransaction();
            foreach (var entity in entities)
            {
                Insert(entity, transaction);
            }
            _restConnector.CommitTransaction(transaction);
        }

        public void Update(Link entity) => Insert(entity);

        public void Update(Link entity, MlTransactionScope transaction) => Insert(entity, transaction);

        public void Update(IEnumerable<Link> entities, MlTransactionScope transaction) => Insert(entities, transaction);

        public void Update(IEnumerable<Link> entities) => Update(entities);

        public IQueryable<Link> Get(Expression expression, long numberOfElements = long.MinValue)
        {
            var query = new FnSubsequence(new CtsSearch("/", expression), numberOfElements).Query;
            return GetFromQuery(query);
        }

        private static string ReadAsString(HttpContent content)
        {
            return content.ReadAsStringAsync().Result;
        }

        private static Link ExtractLinkInfo(XDocument xml, string linkId)
        {
            var linkUri = xml.Descendants().Where(x => x.Name == "link_uri").First().Value;
            var linkKind = xml.Descendants().Where(x => x.Name == "link_kind").First().Value == "Olx" ? OfferType.Olx : OfferType.OtoDom;
            var linkLastUpdate = DateTime.Parse(xml.Descendants().Where(x => x.Name == "last_update").First().Value);
            var linkStatus = Status.New;
            switch (xml.Descendants().Where(x => x.Name == "status").First().Value)
            {
                case "New":
                    linkStatus = Status.New;
                    break;

                case "InProcess":
                    linkStatus = Status.InProcess;
                    break;

                case "Success":
                    linkStatus = Status.Success;
                    break;

                case "Fatal":
                    linkStatus = Status.Fatal;
                    break;
            }
            return new Link()
            {
                Id = linkId,
                Uri = linkUri,
                LinkSourceKind = linkKind,
                LastUpdate = linkLastUpdate,
                Status = linkStatus,
            };
        }

        private IQueryable<Link> GetFromQuery(string query)
        {
            var response = _restConnector.Submit(query);

            if (!response.Content.IsMimeMultipartContent())
                return new List<Link>().AsQueryable();

            var content = response.Content.ReadAsMultipartAsync().Result.Contents;
            var result = new List<Link>();
            foreach (var data in content)
            {
                var text = ReadAsString(data);
                var xml = XDocument.Parse(text);
                var linkId = xml.Descendants().Where(x => x.Name == "link_id").First().Value;
                result.Add(ExtractLinkInfo(xml, linkId));
            }
            return result.AsQueryable();
        }
    }
}