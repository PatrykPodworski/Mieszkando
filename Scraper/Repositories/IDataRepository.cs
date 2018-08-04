using MarklogicDataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace OfferScraper.Repositories
{
    public interface IDataRepository<T> where T : class
    {
        IQueryable<T> GetAll();

        void Insert(T entity);

        void Insert(T entity, MlTransactionScope transaction);

        void Insert(IEnumerable<T> entities, MlTransactionScope transaction);

        void Update(T entity);

        void Update(T entity, MlTransactionScope transaction);

        void Update(IEnumerable<T> entities, MlTransactionScope transaction);

        void Delete(T entity);

        IQueryable<T> SearchFor(Expression<Func<T, bool>> predicate);

        T GetById(int id);
    }
}