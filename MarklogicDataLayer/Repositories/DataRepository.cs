﻿using MarklogicDataLayer.DatabaseConnectors;
using MarklogicDataLayer.XQuery;
using MarklogicDataLayer.XQuery.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Xml.Linq;
using CtsCollectionQuery = MarklogicDataLayer.XQuery.Functions.CtsCollectionQuery;

namespace MarklogicDataLayer.Repositories
{
    public abstract class DataRepository<T> : IDataRepository<T> where T : class
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

        public IQueryable<T> Get(string elementName, string elementValue, string collectionName, long numberOfElements)
        {
            return GetWithExpression(new CtsAndQuery(new CtsCollectionQuery(collectionName), new CtsElementValueQuery(elementName, elementValue)), numberOfElements);
        }

        public IQueryable<T> Get(string elementName, string elementValue, string collectionName, long numberOfElements, long startFrom)
        {
            return GetWithExpression(new CtsAndQuery(new CtsCollectionQuery(collectionName), new CtsElementValueQuery(elementName, elementValue)), numberOfElements, startFrom);
        }

        public virtual IQueryable<T> GetFromCollection(string collectionName, long startFrom = 1)
        {
            return GetWithExpression(new CtsCollectionQuery(collectionName), long.MinValue, startFrom);
        }

        public IQueryable<T> GetWithExpression(Expression expression, long numberOfElements, long startFrom = 1)
        {
            var query = new FnSubsequence(new CtsSearch("/", expression), numberOfElements, startFrom).Query;
            return GetFromQuery(query, _extractionMethod);
        }

        public abstract T GetById(int id);

        public abstract T GetById(string id);

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

        public virtual void Update(T entity, ITransaction transaction) => Insert(entity, transaction);

        public void Update(IEnumerable<T> entities, ITransaction transaction) => Insert(entities, transaction);

        public void Update(IEnumerable<T> entities) => Insert(entities);

        protected static string ReadAsString(HttpContent content)
        {
            return content.ReadAsStringAsync().Result;
        }

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

        public int GetCount(string collection)
        {
            var query = new XdmpEstimate(new CtsSearch("/", new CtsCollectionQuery(collection))).Query;
            var response = RestConnector.Submit(query);

            var result = 0;
            if (!response.Content.IsMimeMultipartContent())
                return result;

            var content = response.Content.ReadAsMultipartAsync().Result.Contents;
            foreach (var data in content)
            {
                var text = ReadAsString(data);
                int.TryParse(text, out result);
            }
            return result;
        }
    }
}