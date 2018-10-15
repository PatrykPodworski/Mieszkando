﻿using MarklogicDataLayer.Constants;
using MarklogicDataLayer.DatabaseConnectors;
using MarklogicDataLayer.DataStructs;
using MarklogicDataLayer.XQuery;
using MarklogicDataLayer.XQuery.Functions;
using System;
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
            using (var writer = new StringWriter())
            using (var xmlWriter = XmlWriter.Create(writer))
            {
                new XmlSerializer(entity.GetType()).Serialize(writer, entity);
                var serializedOffer = writer.GetStringBuilder().ToString();
                var content = MarklogicContent.Xml($"offer_{entity.Id}", serializedOffer, new[] { OfferConstants.CollectionName });
                RestConnector.Insert(content, transaction.GetScope());
            }
        }

        private static Offer ExtractOfferInfo(XDocument xml)
        {
            var offerId = xml.Descendants().First(x => x.Name == OfferConstants.OfferId).Value;
            var offerTitle = xml.Descendants().First(x => x.Name == OfferConstants.Title).Value;
            var offerDistrict = xml.Descendants().First(x => x.Name == OfferConstants.District).Value;
            var offerCost = xml.Descendants().First(x => x.Name == OfferConstants.Cost).Value;
            var offerBonusCost = xml.Descendants().First(x => x.Name == OfferConstants.BonusCost).Value;
            var offerRooms = xml.Descendants().First(x => x.Name == OfferConstants.Rooms).Value;
            var offerArea = xml.Descendants().First(x => x.Name == OfferConstants.Area).Value;
            var offerDateOfPosting = xml.Descendants().First(x => x.Name == OfferConstants.DateOfPosting).Value;
            var offerDateOfScraping = xml.Descendants().First(x => x.Name == OfferConstants.DateOfScraping).Value;
            var offerLatitude = xml.Descendants().First(x => x.Name == OfferConstants.Latitude).Value;
            var offerLongitude = xml.Descendants().First(x => x.Name == OfferConstants.Longitude).Value;

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
            };
        }

        public override IQueryable<Offer> GetAll()
        {
            return GetAllFromCollection(OfferConstants.CollectionName);
        }
    }
}