using MarklogicDataLayer.Constants;
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
    public class DatabaseUtilityRepository : DataRepository<MarklogicDataLayer.DataStructs.Utility>
    {
        public DatabaseUtilityRepository(IDatabaseConnectionSettings databaseConnectionSettings) : base(databaseConnectionSettings, null)
        {
        }

        public override void Delete(MarklogicDataLayer.DataStructs.Utility entity)
        {
            throw new System.NotImplementedException();
        }

        public override MarklogicDataLayer.DataStructs.Utility GetById(int id)
        {
            throw new System.NotImplementedException();
        }

        public MarklogicDataLayer.DataStructs.Utility GetByKind(OfferType kind)
        {
            var utilityKind = kind == OfferType.Olx ? OfferTypeConstants.Olx : OfferTypeConstants.OtoDom;
            var query = new FnDoc(new MlUri($"utility_{utilityKind}", MlUriDocumentType.Xml)).Query;

            var response = RestConnector.Submit(query);

            if (!response.Content.IsMimeMultipartContent())
                return null;

            var content = response.Content.ReadAsMultipartAsync().Result.Contents;
            foreach (var data in content)
            {
                var text = data.ReadAsStringAsync().Result;
                if (string.IsNullOrEmpty(text))
                {
                    return null;
                }

                var xml = XDocument.Parse(text);
                var dateOfLastScraping = DateTime.Parse(
                    xml.Descendants().First(x => x.Name == UtilityConstants.DateOfLastScraping).Value);
                var result = new MarklogicDataLayer.DataStructs.Utility()
                {
                    DateOfLastScraping = dateOfLastScraping,
                    Type = kind,
                };

                return result;
            }

            return null;
        }

        public override void Insert(MarklogicDataLayer.DataStructs.Utility entity, ITransaction transaction)
        {
            var utilityKind = entity.Type == OfferType.Olx ? OfferTypeConstants.Olx : OfferTypeConstants.OtoDom;
            using (var writer = new StringWriter())
            using (var xmlWriter = XmlWriter.Create(writer))
            {
                new XmlSerializer(entity.GetType()).Serialize(writer, entity);
                var serializedOffer = writer.GetStringBuilder().ToString();
                var content = MarklogicContent.Xml($"utility_{utilityKind}", serializedOffer);
                RestConnector.Insert(content, transaction.GetScope());
            }
        }
    }
}