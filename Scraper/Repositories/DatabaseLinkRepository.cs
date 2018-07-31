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
            var linkKind = entity.LinkSourceKind == OfferType.Olx ? "Olx" : "OtoDom";
            using (var writer = new StringWriter())
            using (var xmlWriter = XmlWriter.Create(writer))
            {
                new XmlSerializer(entity.GetType()).Serialize(writer, entity);
                var serializedLink = writer.GetStringBuilder().ToString();
                var content = MarklogicContent.Xml($"{linkKind}_link_{entity.Id}", serializedLink, new[] { linkKind });
                _restConnector.Insert(content);
            }
        }

        public IQueryable<Link> SearchFor(Expression<Func<Link, bool>> predicate)
        {
            throw new NotImplementedException();
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
                Uri = linkUri,
                LinkSourceKind = linkKind,
                LastUpdate = linkLastUpdate,
                Status = linkStatus,
            };
        }
    }
}