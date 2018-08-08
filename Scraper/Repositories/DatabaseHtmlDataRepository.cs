using MarklogicDataLayer;
using MarklogicDataLayer.DatabaseConnectors;
using MarklogicDataLayer.DataStructs;
using MarklogicDataLayer.XQuery;
using MarklogicDataLayer.XQuery.Functions;
using OfferScraper.Constants;
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
            var query = new CtsSearch("/", new CtsElementValueQuery("html_data_id", id.ToString())).Query;
            var response = RestConnector.Submit(query);

            if (!response.Content.IsMimeMultipartContent())
                return null;

            var content = response.Content.ReadAsMultipartAsync().Result.Contents;
            foreach (var data in content)
            {
                var text = data.ReadAsStringAsync().Result;
                var xml = XDocument.Parse(text);
                var htmlDataId = xml.Descendants().Where(x => x.Name == "html_data_id").First().Value;
                if (htmlDataId == id.ToString())
                {
                    return ExtractHtmlDataInfo(xml);
                }
            }

            return null;
        }

        public override void Insert(HtmlData entity, ITransaction transaction)
        {
            var offerType = entity.OfferType == OfferType.Olx ? HtmlDataConstants.OlxHtmlData : HtmlDataConstants.OtoDomHtmlData;
            using (var writer = new StringWriter())
            using (var xmlWriter = XmlWriter.Create(writer))
            {
                new XmlSerializer(entity.GetType()).Serialize(writer, entity);
                var serializedHtmlData = writer.GetStringBuilder().ToString();
                var content = MarklogicContent.Xml($"{offerType}_{entity.Id}", serializedHtmlData, new[] { offerType });
                RestConnector.Insert(content, transaction.GetScope());
            }
        }

        private static HtmlData ExtractHtmlDataInfo(XDocument xml)
        {
            var htmlDataId = xml.Descendants().Where(x => x.Name == "html_data_id").First().Value;
            var htmlDataOfferContent = xml.Descendants().Where(x => x.Name == "offer_content").First().Value;
            var htmlDataOfferType = xml.Descendants().Where(x => x.Name == "offer_type").First().Value == "Olx" ? OfferType.Olx : OfferType.OtoDom;
            var htmlDataLastUpdate = DateTime.Parse(xml.Descendants().Where(x => x.Name == "last_update").First().Value);
            var htmlDataStatus = Status.New;
            switch (xml.Descendants().Where(x => x.Name == "status").First().Value)
            {
                case "New":
                    htmlDataStatus = Status.New;
                    break;

                case "InProgress":
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
    }
}