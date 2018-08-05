using MarklogicDataLayer.XQuery;
using OfferScraper.Commands.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace OfferScraper.Repositories
{
    public class DatabaseCommandRepository : IDataRepository<ICommand>
    {
        public void Delete(ICommand entity)
        {
            throw new System.NotImplementedException();
        }

        public IQueryable<ICommand> Get(Expression expression, long numberOfElements)
        {
            throw new System.NotImplementedException();
        }

        public IQueryable<ICommand> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public ICommand GetById(int id)
        {
            throw new System.NotImplementedException();
        }

        public ITransaction GetTransaction()
        {
            throw new System.NotImplementedException();
        }

        public void Insert(ICommand entity)
        {
            throw new System.NotImplementedException();
        }

        public void Insert(ICommand entity, ITransaction transaction)
        {
            throw new System.NotImplementedException();
        }

        public void Insert(IEnumerable<ICommand> entities, ITransaction transaction)
        {
            throw new System.NotImplementedException();
        }

        public void Insert(IEnumerable<ICommand> entities)
        {
            throw new System.NotImplementedException();
        }

        public void Update(ICommand entity)
        {
            throw new System.NotImplementedException();
        }

        public void Update(ICommand entity, ITransaction transaction)
        {
            throw new System.NotImplementedException();
        }

        public void Update(IEnumerable<ICommand> entities, ITransaction transaction)
        {
            throw new System.NotImplementedException();
        }

        public void Update(IEnumerable<ICommand> entities)
        {
            throw new System.NotImplementedException();
        }
    }
}