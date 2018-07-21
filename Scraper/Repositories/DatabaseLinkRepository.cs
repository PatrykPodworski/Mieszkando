using OfferScrapper.DataStructs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;

namespace OfferScrapper.Repositories
{
    public class DatabaseLinkRepository : IDataRepository<Link>
    {
        public void Delete(Link entity)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Link> GetAll()
        {
            throw new NotImplementedException();
        }

        public Link GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Insert(Link entity)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Link> SearchFor(Expression<Func<Link, bool>> predicate)
        {
            throw new NotImplementedException();
        }
    }
}
