using System.Collections.Generic;
using System.Linq;

namespace MarklogicDataLayer.Repositories
{
    public interface IDataRepository<T> where T : class
    {
        void Insert(T entity);

        void Insert(T entity, ITransaction transaction);

        void Insert(IEnumerable<T> entities, ITransaction transaction);

        void Insert(IEnumerable<T> entities);

        void Update(T entity);

        void Update(T entity, ITransaction transaction);

        void Update(IEnumerable<T> entities, ITransaction transaction);

        void Update(IEnumerable<T> entities);

        void Delete(T entity);

        IQueryable<T> Get(string elementName, string elementValue, string collectionName, long numberOfElements);

        IQueryable<T> Get(string elementName, string elementValue, string collectionName, long numberOfElements, long startFrom);

        IQueryable<T> GetFromCollection(string collectionName, long startFrom);

        T GetById(int id);

        ITransaction GetTransaction();

        int GetCount(string collection);
    }
}