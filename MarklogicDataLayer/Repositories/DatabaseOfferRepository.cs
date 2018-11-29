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
    public class DatabaseOfferRepository : DataRepository<Offer>
    {
        public DatabaseOfferRepository(IDatabaseConnectionSettings databaseConnectionSettings) : base(databaseConnectionSettings, ExtractOfferInfo)
        {
        }

        public override void Delete(Offer entity)
        {
            var query = new XdmpDocumentDelete(new MlUri($"offer_{entity.Id}", MlUriDocumentType.Xml)).Query;
            RestConnector.Submit(query);
        }

        public override Offer GetById(int id)
        {
            var query = new CtsSearch("/", new CtsElementValueQuery(OfferConstants.OfferId, id.ToString())).Query;
            var response = RestConnector.Submit(query);

            if (!response.Content.IsMimeMultipartContent())
                return null;

            var content = response.Content.ReadAsMultipartAsync().Result.Contents;
            foreach (var data in content)
            {
                var text = data.ReadAsStringAsync().Result;
                var xml = XDocument.Parse(text);
                var offerId = xml.Descendants().Where(x => x.Name == OfferConstants.OfferId).First().Value;
                if (offerId == id.ToString())
                {
                    return ExtractOfferInfo(xml);
                }
            }

            return null;
        }

        public override void Insert(Offer entity, ITransaction transaction)
        {
            entity.DateOfScraping = DateTime.Now.ToShortDateString();
            entity.TotalCost = entity.Cost + entity.BonusCost;
            using (var writer = new StringWriter())
            using (var xmlWriter = XmlWriter.Create(writer))
            {
                new XmlSerializer(entity.GetType()).Serialize(writer, entity);
                var serializedOffer = writer.GetStringBuilder().ToString();
                var content = MarklogicContent.Xml($"offer_{entity.Id}", serializedOffer, new[] { OfferConstants.CollectionName });
                RestConnector.Insert(content, transaction.GetScope());
            }
        }

        public override IQueryable<Offer> GetFromCollection(string collectionName = OfferConstants.CollectionName, long startFrom = 1)
        {
            return base.GetFromCollection(collectionName, startFrom);
        }

        private static Offer ExtractOfferInfo(XDocument xml)
        {
            var offerId = xml.Descendants().First(x => x.Name == OfferConstants.OfferId).Value;
            var offerTitle = xml.Descendants().First(x => x.Name == OfferConstants.Title).Value;
            var offerDistrict = xml.Descendants().First(x => x.Name == OfferConstants.District).Value;
            double.TryParse(xml.Descendants().First(x => x.Name == OfferConstants.Cost).Value, NumberStyles.Any, CultureInfo.InvariantCulture, out var offerCost);
            double.TryParse(xml.Descendants().First(x => x.Name == OfferConstants.BonusCost).Value, NumberStyles.Any, CultureInfo.InvariantCulture, out var offerBonusCost);
            int.TryParse(xml.Descendants().First(x => x.Name == OfferConstants.Rooms).Value, NumberStyles.Any, CultureInfo.InvariantCulture, out var offerRooms);
            double.TryParse(xml.Descendants().First(x => x.Name == OfferConstants.Area).Value, NumberStyles.Any, CultureInfo.InvariantCulture, out var offerArea);
            var offerDateOfPosting = xml.Descendants().First(x => x.Name == OfferConstants.DateOfPosting).Value;
            var offerDateOfScraping = xml.Descendants().First(x => x.Name == OfferConstants.DateOfScraping).Value;
            double.TryParse(xml.Descendants().First(x => x.Name == OfferConstants.Latitude).Value, NumberStyles.Any, CultureInfo.InvariantCulture, out var offerLatitude);
            double.TryParse(xml.Descendants().First(x => x.Name == OfferConstants.Longitude).Value, NumberStyles.Any, CultureInfo.InvariantCulture, out var offerLongitude);
            var offerLink = xml.Descendants().First(x => x.Name == OfferConstants.Link).Value;
            double.TryParse(xml.Descendants().First(x => x.Name == OfferConstants.TotalCost).Value, NumberStyles.Any, CultureInfo.InvariantCulture, out var offerTotalCost);
            var offerRegionId = xml.Descendants().FirstOrDefault(x => x.Name == OfferConstants.RegionId)?.Value;
            var offerType = OfferType.Olx;
            switch (xml.Descendants().First(x => x.Name == OfferConstants.OfferType).Value)
            {
                case OfferTypeConstants.Olx:
                    offerType = OfferType.Olx;
                    break;

                case OfferTypeConstants.OtoDom:
                    offerType = OfferType.OtoDom;
                    break;

                case OfferTypeConstants.Outdated:
                    offerType = OfferType.Outdated;
                    break;
            }

            return new Offer
            {
                Id = offerId,
                Title = offerTitle,
                District = offerDistrict,
                Cost = offerCost,
                BonusCost = offerBonusCost,
                Rooms = offerRooms,
                Area = offerArea,
                DateOfPosting = offerDateOfPosting,
                DateOfScraping = offerDateOfScraping,
                Latitude = offerLatitude,
                Longitude = offerLongitude,
                Link = offerLink,
                TotalCost = offerTotalCost,
                RegionId = offerRegionId,
                OfferType = offerType,
            };
        }
    }
}