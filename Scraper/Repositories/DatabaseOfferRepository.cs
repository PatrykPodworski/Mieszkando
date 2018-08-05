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
    public class DatabaseOfferRepository : IDataRepository<Offer>
    {
        private readonly IDatabaseConnectionSettings _databaseConnectionSettings;
        private readonly RestConnector _restConnector;

        public DatabaseOfferRepository(IDatabaseConnectionSettings databaseConnectionSettings)
        {
            _databaseConnectionSettings = databaseConnectionSettings;
            _restConnector = new RestConnector(_databaseConnectionSettings);
        }

        public void Delete(Offer entity)
        {
            var query = new XdmpDocumentDelete(new MlUri($"offer_{entity.Id}", MlUriDocumentType.Xml)).Query;
            _restConnector.Submit(query);
        }

        public IQueryable<Offer> GetAll()
        {
            var query = new CtsSearch("/", new CtsCollectionQuery(new[] { "Offers" })).Query;
            return GetFromQuery(query);
        }

        public Offer GetById(int id)
        {
            var query = new CtsSearch("/", new CtsElementValueQuery("offer_id", id.ToString())).Query;
            var response = _restConnector.Submit(query);

            if (!response.Content.IsMimeMultipartContent())
                return null;

            var content = response.Content.ReadAsMultipartAsync().Result.Contents;
            foreach (var data in content)
            {
                var text = ReadAsString(data);
                var xml = XDocument.Parse(text);
                var offerId = xml.Descendants().Where(x => x.Name == "offer_id").First().Value;
                if (offerId == id.ToString())
                {
                    return ExtractOfferInfo(xml, offerId);
                }
            }

            return null;
        }

        public void Insert(Offer entity)
        {
            using (var writer = new StringWriter())
            using (var xmlWriter = XmlWriter.Create(writer))
            {
                new XmlSerializer(entity.GetType()).Serialize(writer, entity);
                var serializedOffer = writer.GetStringBuilder().ToString();
                var content = MarklogicContent.Xml($"offer_{entity.Id}", serializedOffer, new[] { "Offers" });
                _restConnector.Insert(content);
            }
        }

        public void Insert(Offer entity, MlTransactionScope transaction)
        {
            using (var writer = new StringWriter())
            using (var xmlWriter = XmlWriter.Create(writer))
            {
                new XmlSerializer(entity.GetType()).Serialize(writer, entity);
                var serializedOffer = writer.GetStringBuilder().ToString();
                var content = MarklogicContent.Xml($"offer_{entity.Id}", serializedOffer, new[] { "Offers" });
                _restConnector.Insert(content, transaction);
            }
        }

        public void Insert(IEnumerable<Offer> entities, MlTransactionScope transaction)
        {
            foreach (var entity in entities)
            {
                Insert(entity, transaction);
            }
        }

        public void Insert(IEnumerable<Offer> entities)
        {
            var transaction = _restConnector.BeginTransaction();
            foreach (var entity in entities)
            {
                Insert(entity, transaction);
            }
            _restConnector.CommitTransaction(transaction);
        }

        public void Update(Offer entity) => Insert(entity);

        public void Update(Offer entity, MlTransactionScope transaction) => Insert(entity, transaction);

        public void Update(IEnumerable<Offer> entities, MlTransactionScope transaction) => Insert(entities, transaction);

        public void Update(IEnumerable<Offer> entities) => Insert(entities);

        public IQueryable<Offer> Get(MarklogicDataLayer.XQuery.Expression expression, long numberOfElements = long.MinValue)
        {
            var query = new FnSubsequence(new CtsSearch("/", expression), numberOfElements).Query;
            return GetFromQuery(query);
        }

        private static string ReadAsString(HttpContent content)
        {
            return content.ReadAsStringAsync().Result;
        }

        private static Offer ExtractOfferInfo(XDocument xml, string offerId)
        {
            var offerTitle = xml.Descendants().Where(x => x.Name == "title").First().Value;
            var offerDescription = xml.Descendants().Where(x => x.Name == "description").First().Value;
            var offerDistrict = xml.Descendants().Where(x => x.Name == "district").First().Value;
            var offerCost = xml.Descendants().Where(x => x.Name == "cost").First().Value;
            var offerBonusCost = xml.Descendants().Where(x => x.Name == "bonusCost").First().Value;
            var offerRooms = xml.Descendants().Where(x => x.Name == "rooms").First().Value;
            var offerArea = xml.Descendants().Where(x => x.Name == "area").First().Value;
            var offerDateOfPosting = xml.Descendants().Where(x => x.Name == "dateOfPosting").First().Value;
            var offerDateOfScraping = xml.Descendants().Where(x => x.Name == "dateOfScraping").First().Value;

            return new Offer
            {
                Id = offerId,
                Title = offerTitle,
                Description = offerDescription,
                District = offerDistrict,
                Cost = offerCost,
                BonusCost = offerBonusCost,
                Rooms = offerRooms,
                Area = offerArea,
                DateOfPosting = offerDateOfPosting,
                DateOfScraping = offerDateOfScraping,
            };
        }

        private IQueryable<Offer> GetFromQuery(string query)
        {
            var response = _restConnector.Submit(query);

            if (!response.Content.IsMimeMultipartContent())
                return new List<Offer>().AsQueryable();

            var content = response.Content.ReadAsMultipartAsync().Result.Contents;
            var result = new List<Offer>();
            foreach (var data in content)
            {
                var text = ReadAsString(data);
                var xml = XDocument.Parse(text);
                var offerId = xml.Descendants().Where(x => x.Name == "offer_id").First().Value;
                result.Add(ExtractOfferInfo(xml, offerId));
            }
            return result.AsQueryable();
        }
        public ITransaction GetTransaction()
        {
            return new DatabaseTransaction(_restConnector);
        }
    }
}