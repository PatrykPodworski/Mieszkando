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
    public class DatabaseHtmlDataRepository : DataRepository<HtmlData>
    {
        public DatabaseHtmlDataRepository(IDatabaseConnectionSettings databaseConnectionSettings) : base(databaseConnectionSettings, ExtractHtmlDataInfo)
        {
        }

        public override void Delete(HtmlData entity)
        {
            var offerType = entity.OfferType == OfferType.Olx ? HtmlDataConstants.OlxHtmlData : HtmlDataConstants.OtoDomHtmlData;
            var query = new XdmpDocumentDelete(new MlUri($"{offerType}_{entity.Id}", MlUriDocumentType.Xml)).Query;
            RestConnector.Submit(query);
        }

        public override HtmlData GetById(int id)
        {
            var query = new CtsSearch("/", new CtsElementValueQuery(HtmlDataConstants.HtmlDataId, id.ToString())).Query;
            var response = RestConnector.Submit(query);

            if (!response.Content.IsMimeMultipartContent())
                return null;

            var content = response.Content.ReadAsMultipartAsync().Result.Contents;
            foreach (var data in content)
            {
                var text = data.ReadAsStringAsync().Result;
                var xml = XDocument.Parse(text);
                var htmlDataId = xml.Descendants().Where(x => x.Name == HtmlDataConstants.HtmlDataId).First().Value;
                if (htmlDataId == id.ToString())
                {
                    return ExtractHtmlDataInfo(xml);
                }
            }

            return null;
        }

        public override void Insert(HtmlData entity, ITransaction transaction)
        {
            entity.LastUpdate = DateTime.Now;
            var offerType = entity.OfferType == OfferType.Olx ? HtmlDataConstants.OlxHtmlData : HtmlDataConstants.OtoDomHtmlData;
            using (var writer = new StringWriter())
            using (var xmlWriter = XmlWriter.Create(writer))
            {
                new XmlSerializer(entity.GetType()).Serialize(writer, entity);
                var serializedHtmlData = writer.GetStringBuilder().ToString();
                var content = MarklogicContent.Xml($"{offerType}_{entity.Id}", serializedHtmlData, new[] { offerType, HtmlDataConstants.CollectionName });
                RestConnector.Insert(content, transaction.GetScope());
            }
        }

        public override IQueryable<HtmlData> GetFromCollection(string collectionName = HtmlDataConstants.CollectionName, long startFrom = 1)
        {
            return base.GetFromCollection(collectionName, startFrom);
        }

        private static HtmlData ExtractHtmlDataInfo(XDocument xml)
        {
            var htmlDataId = xml.Descendants().Where(x => x.Name == HtmlDataConstants.HtmlDataId).First().Value;
            var htmlDataOfferContent = xml.Descendants().Where(x => x.Name == HtmlDataConstants.OfferContent).First().Value;
            var htmlDataOfferType = xml.Descendants().Where(x => x.Name == HtmlDataConstants.OfferType).First().Value == OfferTypeConstants.Olx ? OfferType.Olx : OfferType.OtoDom;
            var htmlDataLastUpdate = DateTime.Parse(xml.Descendants().Where(x => x.Name == HtmlDataConstants.LastUpdate).First().Value, CultureInfo.InvariantCulture);
            var htmlDataLink = xml.Descendants().Where(x => x.Name == HtmlDataConstants.Link).First().Value;
            var htmlDataStatus = Status.New;
            switch (xml.Descendants().Where(x => x.Name == HtmlDataConstants.Status).First().Value)
            {
                case StatusConstants.StatusNew:
                    htmlDataStatus = Status.New;
                    break;

                case StatusConstants.StatusInProgress:
                    htmlDataStatus = Status.InProgress;
                    break;

                case StatusConstants.StatusSuccess:
                    htmlDataStatus = Status.Success;
                    break;

                case StatusConstants.StatusFailed:
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
                Link = htmlDataLink,
            };
        }

        public override HtmlData GetById(string id)
        {
            return int.TryParse(id, out var identifier) ? GetById(identifier) : throw new ArgumentException();
        }
    }
}