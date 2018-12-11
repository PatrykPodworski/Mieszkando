using MarklogicDataLayer.SearchQuery.SearchModels;
using MarklogicDataLayer.DatabaseConnectors;
using System.Xml.Linq;
using MarklogicDataLayer.XQuery.Functions;
using MarklogicDataLayer.Constants;
using System.Net.Http;
using System.Linq;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System;

namespace MarklogicDataLayer.Repositories
{
    public class DatabaseSearchCriteriaRepository : DataRepository<SearchModel>
    {
        public DatabaseSearchCriteriaRepository(IDatabaseConnectionSettings databaseConnectionSettings) : base(databaseConnectionSettings, ExtractSearchModelInfo)
        {
        }

        public override void Delete(SearchModel entity)
        {
            var query = new XdmpDocumentDelete(new XQuery.MlUri(entity.Id, XQuery.MlUriDocumentType.Xml)).Query;
            RestConnector.Submit(query);
        }

        public override SearchModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public override SearchModel GetById(string id)
        {
            var query = new CtsSearch("/", new CtsElementValueQuery(SearchModelConstants.Id, id)).Query;
            var response = RestConnector.Submit(query);

            if (!response.Content.IsMimeMultipartContent())
                return null;

            var content = response.Content.ReadAsMultipartAsync().Result.Contents;
            foreach (var data in content)
            {
                var text = data.ReadAsStringAsync().Result;
                var xml = XDocument.Parse(text);
                var modelId = xml.Descendants().Where(x => x.Name == SearchModelConstants.Id).First().Value;
                if (modelId == id.ToString())
                {
                    return ExtractSearchModelInfo(xml);
                }
            }

            return null;
        }

        public override void Insert(SearchModel entity, ITransaction transaction)
        {
            using (var writer = new StringWriter())
            using (var xmlWriter = XmlWriter.Create(writer))
            {
                new XmlSerializer(entity.GetType()).Serialize(writer, entity);
                var serializedModel = writer.GetStringBuilder().ToString();
                var content = MarklogicContent.Xml($"{entity.Id}", serializedModel, new[] { SearchModelConstants.CollectionName });
                RestConnector.Insert(content, transaction.GetScope());
            }
        }

        private static SearchModel ExtractSearchModelInfo(XDocument xml)
        {
            var modelId = xml.Descendants().First(x => x.Name.LocalName == SearchModelConstants.Id).Value;
            var modelMinCost = xml.Descendants().FirstOrDefault(x => x.Name.LocalName == SearchModelConstants.MinCost)?.Value;
            var modelMaxCost = xml.Descendants().FirstOrDefault(x => x.Name.LocalName == SearchModelConstants.MaxCost)?.Value;
            var modelMinArea = xml.Descendants().FirstOrDefault(x => x.Name.LocalName == SearchModelConstants.MinArea)?.Value;
            var modelMaxArea = xml.Descendants().FirstOrDefault(x => x.Name.LocalName == SearchModelConstants.MaxArea)?.Value;
            var modelNumberOfRooms = xml.Descendants().FirstOrDefault(x => x.Name.LocalName == SearchModelConstants.NumberOfRooms)?.Value;

            return new SearchModel
            {
                Id = modelId,
                MinCost = modelMinCost,
                MaxCost = modelMaxCost,
                MinArea = modelMinArea,
                MaxArea = modelMaxArea,
                NumberOfRooms = modelNumberOfRooms
            };
        }
    }
}
