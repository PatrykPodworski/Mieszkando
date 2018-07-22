using MarklogicDataLayer.DataStructs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;
using MarklogicDataLayer.DatabaseConnectors;
using MarklogicDataLayer;
using MarklogicDataLayer.XQuery.Functions;
using MarklogicDataLayer.XQuery;

namespace OfferScrapper.Repositories
{
    public class DatabaseLinkRepository : IDataRepository<Link>
    {
        private readonly IDatabaseConnectionSettings _databaseConnectionSettings;
        private readonly RestConnector _restConnector;

        public DatabaseLinkRepository(IDatabaseConnectionSettings databaseConnectionSettings)
        {
            _databaseConnectionSettings = databaseConnectionSettings;
            _restConnector = new RestConnector(_databaseConnectionSettings);
        }

        public void Delete(Link entity)
        {
            var query = new XdmpDocumentDelete(new MlUri(entity.Id)).Query;
            _restConnector.Submit(query);
        }

        public IQueryable<Link> GetAll()
        {
            var query = new CtsCollectionQuery(new[] { "Olx", "OtoDom" }).Query;
            var response = _restConnector.Submit(query);

            return new List<Link>().AsQueryable();
        }

        public Link GetById(int id)
        {
            var query = new FnDoc(new MlUri(id.ToString())).Query;
            var response = _restConnector.Submit(query);

            return new Link("1","1",LinkKind.Olx);
        }

        public void Insert(Link entity)
        {
            var linkKind = entity.LinkSourceKind == LinkKind.Olx ? "Olx" : "OtoDom";
            var content = new MarklogicContent($"{linkKind}_link_{entity.Id}", entity.Uri, new[] { linkKind });
            _restConnector.Insert(content);
        }

        public IQueryable<Link> SearchFor(Expression<Func<Link, bool>> predicate)
        {
            throw new NotImplementedException();
        }
    }
}
