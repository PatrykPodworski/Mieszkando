using MarklogicDataLayer;
using MarklogicDataLayer.DatabaseConnectors;
using MarklogicDataLayer.XQuery;
using MarklogicDataLayer.XQuery.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Xml.Linq;

namespace OfferScraper.Repositories
{
    public abstract class DataRepository<T> : IDataRepository<T> where T: class
    {
        public IDatabaseConnectionSettings DatabaseConnectionSettings { get; }

        public RestConnector RestConnector { get; }

        private readonly Func<XDocument, T> _extractionMethod;

        public DataRepository(IDatabaseConnectionSettings databaseConnectionSettings, Func<XDocument, T> extractionMethod)
        {
            DatabaseConnectionSettings = databaseConnectionSettings;
            RestConnector = new RestConnector(DatabaseConnectionSettings);
            _extractionMethod = extractionMethod;
        }

        public abstract void Delete(T entity);

        public IQueryable<T> Get(string elementName, string elementValue, long numberOfElements)
        {
            return GetWithExpression(new CtsElementValueQuery(elementName, elementValue), numberOfElements);
        }

        public IQueryable<T> GetAll()
        {
            return GetWithExpression(new EmptyExpression(), long.MinValue);
        }

        public abstract T GetById(int id);

        public ITransaction GetTransaction()
        {
            return new DatabaseTransaction(RestConnector);
        }

        public void Insert(T entity)
        {
            using (var transaction = GetTransaction())
            {
                Insert(entity, transaction);
            }
        }

        public abstract void Insert(T entity, ITransaction transaction);

        public void Insert(IEnumerable<T> entities, ITransaction transaction)
        {
            foreach (var entity in entities)
            {
                Insert(entity, transaction);
            }
        }

        public void Insert(IEnumerable<T> entities)
        {
            using (var transaction = GetTransaction())
            {
                foreach (var entity in entities)
                {
                    Insert(entity, transaction);
                }
            }
        }

        public void Update(T entity) => Insert(entity);

        public void Update(T entity, ITransaction transaction) => Insert(entity, transaction);

        public void Update(IEnumerable<T> entities, ITransaction transaction) => Insert(entities, transaction);

        public void Update(IEnumerable<T> entities) => Insert(entities);

        private IQueryable<T> GetFromQuery(string query, Func<XDocument, T> extractionMethod)
        {
            var response = RestConnector.Submit(query);

            if (!response.Content.IsMimeMultipartContent())
                return new List<T>().AsQueryable();

            var content = response.Content.ReadAsMultipartAsync().Result.Contents;
            var result = new List<T>();
            foreach (var data in content)
            {
                var text = ReadAsString(data);
                var xml = XDocument.Parse(text);
                result.Add(extractionMethod(xml));
            }
            return result.AsQueryable();
        }

        private static string ReadAsString(HttpContent content)
        {
            return content.ReadAsStringAsync().Result;
        }

        private IQueryable<T> GetWithExpression(Expression expression, long numberOfElements)
        {
            var query = new FnSubsequence(new CtsSearch("/", expression), numberOfElements).Query;
            return GetFromQuery(query, _extractionMethod);
        }
    }
}
