using MarklogicDataLayer.XQuery;
using OfferScraper.Commands.Interfaces;
using System.Collections.Generic;
using System.Linq;
using MarklogicDataLayer.DatabaseConnectors;
using System;
using System.Xml.Linq;

namespace OfferScraper.Repositories
{
    public class DatabaseCommandRepository : DataRepository<ICommand>
    {
        public DatabaseCommandRepository(IDatabaseConnectionSettings databaseConnectionSettings) : base(databaseConnectionSettings, null)
        {
        }

        public override void Delete(ICommand entity)
        {
            throw new System.NotImplementedException();
        }

        public override ICommand GetById(int id)
        {
            throw new System.NotImplementedException();
        }

        public override void Insert(ICommand entity, ITransaction transaction)
        {
            throw new System.NotImplementedException();
        }
    }
}