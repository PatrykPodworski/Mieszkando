using MarklogicDataLayer.XQuery;
using OfferScraper.Commands.Interfaces;
using System.Collections.Generic;
using System.Linq;
using MarklogicDataLayer.DatabaseConnectors;
using System;
using System.Xml.Linq;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using MarklogicDataLayer;

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
            var commandType = entity.GetType().ToString().Split(".").Last();
            using (var writer = new StringWriter())
            using (var xmlWriter = XmlWriter.Create(writer))
            {
                new XmlSerializer(entity.GetType()).Serialize(writer, entity);
                var serializedCommand = writer.GetStringBuilder().ToString();
                var content = MarklogicContent.Xml($"{commandType}_{Guid.NewGuid().ToString()}", serializedCommand, new[] { commandType });
                RestConnector.Insert(content, transaction.GetScope());
            }
        }
    }
}