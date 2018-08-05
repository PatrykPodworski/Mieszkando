using OfferScraper.Commands.Interfaces;

namespace OfferScraper.Repositories
{
    internal class DatabaseCommandRepository : IDataRepository<ICommand>
    {
        public void Delete(ICommand entity)
        {
            throw new System.NotImplementedException();
        }

        public System.Linq.IQueryable<ICommand> GetAll()
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

        public void Insert(System.Collections.Generic.IEnumerable<ICommand> entities, ITransaction transaction)
        {
            throw new System.NotImplementedException();
        }

        public System.Linq.IQueryable<ICommand> SearchFor(System.Linq.Expressions.Expression<System.Func<ICommand, bool>> predicate)
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

        public void Update(System.Collections.Generic.IEnumerable<ICommand> entities, ITransaction transaction)
        {
            throw new System.NotImplementedException();
        }
    }
}