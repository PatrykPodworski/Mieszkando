using MarklogicDataLayer.Constants;
using MarklogicDataLayer.DatabaseConnectors;
using MarklogicDataLayer.DataStructs;
using MarklogicDataLayer.XQuery;
using MarklogicDataLayer.XQuery.Functions;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace MarklogicDataLayer.Repositories
{
    public class DatabaseLinkRepository : DataRepository<Link>
    {
        public DatabaseLinkRepository(IDatabaseConnectionSettings databaseConnectionSettings) : base(databaseConnectionSettings, ExtractLinkInfo)
        {
        }

        public override void Delete(Link entity)
        {
            var linkKind = entity.LinkSourceKind == OfferType.Olx ? OfferTypeConstants.Olx : OfferTypeConstants.OtoDom;
            var query = new XdmpDocumentDelete(new MlUri($"{linkKind}_link_{entity.Id}", MlUriDocumentType.Xml)).Query;
            RestConnector.Submit(query);
        }

        public override Link GetById(int id)
        {
            var query = new CtsSearch("/", new CtsElementValueQuery(LinkConstants.LinkId, id.ToString())).Query;
            var response = RestConnector.Submit(query);

            if (!response.Content.IsMimeMultipartContent())
                return null;

            var content = response.Content.ReadAsMultipartAsync().Result.Contents;
            foreach (var data in content)
            {
                var text = data.ReadAsStringAsync().Result;
                var xml = XDocument.Parse(text);
                var linkId = xml.Descendants().Where(x => x.Name == LinkConstants.LinkId).First().Value;
                if (linkId == id.ToString())
                {
                    return ExtractLinkInfo(xml);
                }
            }

            return null;
        }

        public override Link GetById(string id)
        {
            return int.TryParse(id, out var identifier) ? GetById(identifier) : throw new ArgumentException();
        }

        public override void Insert(Link entity, ITransaction transaction)
        {
            entity.LastUpdate = DateTime.Now;
            var linkKind = entity.LinkSourceKind == OfferType.Olx ? OfferTypeConstants.Olx : OfferTypeConstants.OtoDom;
            using (var writer = new StringWriter())
            using (var xmlWriter = XmlWriter.Create(writer))
            {
                new XmlSerializer(entity.GetType()).Serialize(writer, entity);
                var serializedLink = writer.GetStringBuilder().ToString();
                var content = MarklogicContent.Xml($"{linkKind}_link_{entity.Id}", serializedLink, new[] { linkKind, LinkConstants.CollectionName });
                RestConnector.Insert(content, transaction.GetScope());
            }
        }

        public override IQueryable<Link> GetFromCollection(string collectionName = LinkConstants.CollectionName, long startFrom = 1)
        {
            return base.GetFromCollection(collectionName, startFrom);
        }

        private static Link ExtractLinkInfo(XDocument xml)
        {
            var linkId = xml.Descendants().Where(x => x.Name == LinkConstants.LinkId).First().Value;
            var linkUri = xml.Descendants().Where(x => x.Name == LinkConstants.LinkUri).First().Value;
            var linkKind = xml.Descendants().Where(x => x.Name == LinkConstants.LinkKind).First().Value == OfferTypeConstants.Olx ? OfferType.Olx : OfferType.OtoDom;
            var linkLastUpdate = DateTime.Parse(xml.Descendants().Where(x => x.Name == LinkConstants.LastUpdate).First().Value, CultureInfo.InvariantCulture);
            var linkStatus = Status.New;
            switch (xml.Descendants().Where(x => x.Name == LinkConstants.Status).First().Value)
            {
                case StatusConstants.StatusNew:
                    linkStatus = Status.New;
                    break;

                case StatusConstants.StatusInProgress:
                    linkStatus = Status.InProgress;
                    break;

                case StatusConstants.StatusSuccess:
                    linkStatus = Status.Success;
                    break;

                case StatusConstants.StatusFailed:
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