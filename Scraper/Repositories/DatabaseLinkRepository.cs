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
    public class DatabaseLinkRepository : DataRepository<Link>
    {
        public DatabaseLinkRepository(IDatabaseConnectionSettings databaseConnectionSettings) : base(databaseConnectionSettings, ExtractLinkInfo)
        {
        }

        public override void Delete(Link entity)
        {
            var linkKind = entity.LinkSourceKind == OfferType.Olx ? "Olx" : "OtoDom";
            var query = new XdmpDocumentDelete(new MlUri($"{linkKind}_link_{entity.Id}", MlUriDocumentType.Xml)).Query;
            RestConnector.Submit(query);
        }

        public override Link GetById(int id)
        {
            var query = new CtsSearch("/", new CtsElementValueQuery("link_id", id.ToString())).Query;
            var response = RestConnector.Submit(query);

            if (!response.Content.IsMimeMultipartContent())
                return null;

            var content = response.Content.ReadAsMultipartAsync().Result.Contents;
            foreach (var data in content)
            {
                var text = data.ReadAsStringAsync().Result;
                var xml = XDocument.Parse(text);
                var linkId = xml.Descendants().Where(x => x.Name == "link_id").First().Value;
                if (linkId == id.ToString())
                {
                    return ExtractLinkInfo(xml);
                }
            }

            return null;
        }

        public override void Insert(Link entity, ITransaction transaction)
        {
            var linkKind = entity.LinkSourceKind == OfferType.Olx ? "Olx" : "OtoDom";
            using (var writer = new StringWriter())
            using (var xmlWriter = XmlWriter.Create(writer))
            {
                new XmlSerializer(entity.GetType()).Serialize(writer, entity);
                var serializedLink = writer.GetStringBuilder().ToString();
                var content = MarklogicContent.Xml($"{linkKind}_link_{entity.Id}", serializedLink, new[] { linkKind });
                RestConnector.Insert(content, transaction.GetScope());
            }
        }

        private static Link ExtractLinkInfo(XDocument xml)
        {
            var linkId = xml.Descendants().Where(x => x.Name == "link_id").First().Value;
            var linkUri = xml.Descendants().Where(x => x.Name == "link_uri").First().Value;
            var linkKind = xml.Descendants().Where(x => x.Name == "link_kind").First().Value == "Olx" ? OfferType.Olx : OfferType.OtoDom;
            var linkLastUpdate = DateTime.Parse(xml.Descendants().Where(x => x.Name == "last_update").First().Value);
            var linkStatus = Status.New;
            switch (xml.Descendants().Where(x => x.Name == "status").First().Value)
            {
                case "New":
                    linkStatus = Status.New;
                    break;

                case "InProgress":
                    linkStatus = Status.InProgress;
                    break;

                case "Success":
                    linkStatus = Status.Success;
                    break;

                case "Fatal":
                    linkStatus = Status.Failed;
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
    }
}