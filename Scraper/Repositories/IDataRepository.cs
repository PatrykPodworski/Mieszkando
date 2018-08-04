using MarklogicDataLayer;
using MarklogicDataLayer.XQuery;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OfferScraper.Repositories
{
    public interface IDataRepository<T> where T : class
    {
        IQueryable<T> GetAll();

        void Insert(T entity);

        void Insert(T entity, MlTransactionScope transaction);

        void Insert(IEnumerable<T> entities, MlTransactionScope transaction);

        void Insert(IEnumerable<T> entities);

        void Update(T entity);

        void Update(T entity, MlTransactionScope transaction);

        void Update(IEnumerable<T> entities, MlTransactionScope transaction);

        void Update(IEnumerable<T> entities);

        void Delete(T entity);

        IQueryable<T> Get(Expression expression, long numberOfElements);

        T GetById(int id);
    }
}