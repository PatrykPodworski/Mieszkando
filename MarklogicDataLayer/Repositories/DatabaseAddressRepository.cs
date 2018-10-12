using System;
using System.Linq;
using System.Xml.Linq;
using MarklogicDataLayer.DatabaseConnectors;
using MarklogicDataLayer.DataStructs;
using MarklogicDataLayer.XQuery.Functions;
using MarklogicDataLayer.XQuery;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using MarklogicDataLayer.Constants;
using System.Collections.Generic;
using System.Net.Http;

namespace MarklogicDataLayer.Repositories
{
    public class DatabaseAddressRepository : DataRepository<Address>
    {
        public DatabaseAddressRepository(IDatabaseConnectionSettings databaseConnectionSettings) : base(databaseConnectionSettings, ExtractAddressInfo)
        {
        }

        public override void Delete(Address entity)
        {
            var query = new XdmpDocumentDelete(new MlUri($"address_{entity.Id}", MlUriDocumentType.Xml)).Query;
            RestConnector.Submit(query);
        }

        public override IQueryable<Address> GetAll()
        {
            return GetAllFromCollection(AddressConstants.CollectionName);
        }

        public override Address GetById(int id)
        {
            var query = new CtsSearch("/", new CtsElementValueQuery(AddressConstants.Id, id.ToString())).Query;
            var response = RestConnector.Submit(query);

            if (!response.Content.IsMimeMultipartContent())
                return null;

            var content = response.Content.ReadAsMultipartAsync().Result.Contents;
            foreach (var data in content)
            {
                var text = data.ReadAsStringAsync().Result;
                var xml = XDocument.Parse(text);
                var addressId = xml.Descendants().First(x => x.Name == AddressConstants.Id).Value;
                if (addressId == id.ToString())
                {
                    return ExtractAddressInfo(xml);
                }
            }

            return null;
        }

        public override void Insert(Address entity, ITransaction transaction)
        {
            using (var writer = new StringWriter())
            using (var xmlWriter = XmlWriter.Create(writer))
            {
                new XmlSerializer(entity.GetType()).Serialize(writer, entity);
                var serializedAddress = writer.GetStringBuilder().ToString();
                var content = MarklogicContent.Xml($"address_{entity.Id}", serializedAddress, new[] { AddressConstants.CollectionName });
                RestConnector.Insert(content, transaction.GetScope());
            }
        }

        private static Address ExtractAddressInfo(XDocument xml)
        {
            var addressId = xml.Descendants().First(x => x.Name == AddressConstants.Id).Value;
            var addressCountry = xml.Descendants().First(x => x.Name == AddressConstants.Country).Value;
            var addressCountryCode = xml.Descendants().First(x => x.Name == AddressConstants.CountryCode).Value;
            var addressCountrySecondarySubdivision = xml.Descendants().First(x => x.Name == AddressConstants.CountrySecondarySubdivision).Value;
            var addressCountrySubdivision = xml.Descendants().First(x => x.Name == AddressConstants.CountrySubdivision).Value;
            var addressMunicipality = xml.Descendants().First(x => x.Name == AddressConstants.Municipality).Value;
            var addressMunicipalitySubdivision = xml.Descendants().First(x => x.Name == AddressConstants.MunicipalitySubdivision).Value;
            var addressPostalCode = xml.Descendants().First(x => x.Name == AddressConstants.PostalCode).Value;
            var addressRouteNumbers = xml.Descendants().First(x => x.Name == AddressConstants.RouteNumbers).Value;
            var addressStreet = xml.Descendants().First(x => x.Name == AddressConstants.Street).Value;
            return new Address
            {
                Id = addressId,
                Country = addressCountry,
                CountryCode = addressCountryCode,
                CountrySecondarySubdivision = addressCountrySecondarySubdivision,
                CountrySubdivision = addressCountrySubdivision,
                Municipality = addressMunicipality,
                MunicipalitySubdivision = addressMunicipalitySubdivision,
                PostalCode = addressPostalCode,
                RouteNumbers = new List<string>
                {

                },
                Street = addressStreet,
            };
        }
    }
}
