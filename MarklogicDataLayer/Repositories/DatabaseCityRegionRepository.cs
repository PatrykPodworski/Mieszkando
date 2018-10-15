﻿using MarklogicDataLayer.DataStructs;
using System.Linq;
using MarklogicDataLayer.DatabaseConnectors;
using System.Xml.Linq;
using MarklogicDataLayer.XQuery.Functions;
using MarklogicDataLayer.XQuery;
using MarklogicDataLayer.Constants;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Net.Http;

namespace MarklogicDataLayer.Repositories
{
    public class DatabaseCityRegionRepository : DataRepository<CityRegion>
    {
        public DatabaseCityRegionRepository(IDatabaseConnectionSettings databaseConnectionSettings) : base(databaseConnectionSettings, ExtractCityRegionInfo)
        {
        }

        public override void Delete(CityRegion entity)
        {
            var query = new XdmpDocumentDelete(new MlUri($"{CityRegionConstants.Id}_{entity.Id}", MlUriDocumentType.Xml)).Query;
            RestConnector.Submit(query);
        }

        public override IQueryable<CityRegion> GetAll()
        {
            return GetAllFromCollection(CityRegionConstants.CollectionName);
        }

        public override CityRegion GetById(int id)
        {
            var query = new CtsSearch("/", new CtsElementValueQuery(CityRegionConstants.Id, id.ToString())).Query;
            var response = RestConnector.Submit(query);

            if (!response.Content.IsMimeMultipartContent())
                return null;

            var content = response.Content.ReadAsMultipartAsync().Result.Contents;
            foreach (var data in content)
            {
                var text = data.ReadAsStringAsync().Result;
                var xml = XDocument.Parse(text);
                var regionId = xml.Descendants().First(x => x.Name == CityRegionConstants.Id).Value;
                if (regionId == id.ToString())
                {
                    return ExtractCityRegionInfo(xml);
                }
            }

            return null;
        }

        public override void Insert(CityRegion entity, ITransaction transaction)
        {
            using (var writer = new StringWriter())
            using (var xmlWriter = XmlWriter.Create(writer))
            {
                new XmlSerializer(entity.GetType()).Serialize(writer, entity);
                var serializedCityRegion = writer.GetStringBuilder().ToString();
                var content = MarklogicContent.Xml($"{CityRegionConstants.Id}_{entity.Id}", serializedCityRegion, new[] { CityRegionConstants.CollectionName });
                RestConnector.Insert(content, transaction.GetScope());
            }
        }

        private static CityRegion ExtractCityRegionInfo(XDocument xml)
        {
            var cityRegionId = xml.Descendants().First(x => x.Name == CityRegionConstants.Id).Value;
            var cityLatitude = xml.Descendants().First(x => x.Name == CityRegionConstants.Latitude).Value;
            var cityLatitudeSize = xml.Descendants().First(x => x.Name == CityRegionConstants.LatitudeSize).Value;
            var cityLongitude = xml.Descendants().First(x => x.Name == CityRegionConstants.Longitude).Value;
            var cityLongitudeSize = xml.Descendants().First(x => x.Name == CityRegionConstants.LongitudeSize).Value;

            return new CityRegion
            {
                Id = cityRegionId,
                Latitude = cityLatitude,
                LatitudeSize = cityLatitudeSize,
                Longitude = cityLongitude,
                LongitudeSize = cityLongitudeSize,
            };
        }
    }
}