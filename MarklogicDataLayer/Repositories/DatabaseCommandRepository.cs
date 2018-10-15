using MarklogicDataLayer.Commands.Implementation;
using MarklogicDataLayer.Commands.Interfaces;
using MarklogicDataLayer.Constants;
using MarklogicDataLayer.DatabaseConnectors;
using MarklogicDataLayer.DataStructs;
using MarklogicDataLayer.XQuery;
using MarklogicDataLayer.XQuery.Functions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace MarklogicDataLayer.Repositories
{
    public class DatabaseCommandRepository : DataRepository<ICommand>
    {
        private const string TimestampFormat = "yyyy-MM-ddTHH:mm:ss.fffffffzzz";

        public DatabaseCommandRepository(IDatabaseConnectionSettings databaseConnectionSettings) : base(databaseConnectionSettings, ExtractCommandInfo)
        {
        }

        public override void Delete(ICommand entity)
        {
            var flwor = new Flwor();
            flwor.Let(new VariableName("doc"), new CtsSearch("/",
                    new CtsElementValueQuery(CommandConstants.CommandId,
                        (entity as BaseCommand)?.Id)));
            flwor.Return(new XdmpNodeDelete(new VariableName("doc").Query));
            RestConnector.Submit(flwor.Query);
        }

        public override ICommand GetById(int id)
        {
            throw new System.NotImplementedException();
        }

        public override void Insert(ICommand entity, ITransaction transaction)
        {
            var presentDate = DateTime.Now;
            entity.SetDateOfCreation(presentDate);
            entity.SetLastModifiedDate(presentDate);
            InsertEntity(entity, transaction);
        }

        public override void Update(ICommand entity, ITransaction transaction)
        {
            var presentDate = DateTime.Now;
            entity.SetLastModifiedDate(presentDate);
            InsertEntity(entity, transaction);
        }

        private void InsertEntity(ICommand entity, ITransaction transaction)
        {
            var commandType = entity.GetType().ToString().Split(".").Last();
            using (var writer = new StringWriter())
            using (var xmlWriter = XmlWriter.Create(writer))
            {
                new XmlSerializer(entity.GetType()).Serialize(writer, entity);
                var serializedCommand = writer.GetStringBuilder().ToString();
                var content = MarklogicContent.Xml($"{commandType}_{entity.GetId()}", serializedCommand, new[] { commandType, CommandConstants.CollectionName });
                RestConnector.Insert(content, transaction.GetScope());
            }
        }

        private static ICommand ExtractCommandInfo(XDocument xml)
        {
            var elements = xml.Descendants().ToList();
            var creationDate = Convert.ToDateTime(GetElementValueFromXml(elements, CommandConstants.CreationDate));
            var lastModified = Convert.ToDateTime(GetElementValueFromXml(elements, CommandConstants.LastModified));
            var status = GetElementValueFromXml(elements, CommandConstants.Status);
            var id = GetElementValueFromXml(elements, CommandConstants.CommandId);
            switch (xml.Root.Name.LocalName)
            {
                case CommandConstants.ExtractDataCommand:
                    var numberOfSamples = int.Parse(GetElementValueFromXml(elements, CommandConstants.NumberOfSamples));
                    return new ExtractDataCommand()
                    {
                        Id = id,
                        DateOfCreation = creationDate,
                        LastModified = lastModified,
                        Status = GetStatus(status),
                        NumberOfSamples = numberOfSamples,
                    };

                case CommandConstants.GatherDataCommand:
                    var numberOfLinks = int.Parse(GetElementValueFromXml(elements, CommandConstants.NumberOfLinks));
                    return new GatherDataCommand()
                    {
                        Id = id,
                        DateOfCreation = creationDate,
                        LastModified = lastModified,
                        Status = GetStatus(status),
                        NumberOfLinks = numberOfLinks,
                    };

                case CommandConstants.GetLinksCommand:
                    var offerType = GetElementValueFromXml(elements, CommandConstants.OfferType) == OfferTypeConstants.Olx ? OfferType.Olx : OfferType.OtoDom;
                    return new GetLinksCommand()
                    {
                        Id = id,
                        DateOfCreation = creationDate,
                        LastModified = lastModified,
                        Status = GetStatus(status),
                        Type = offerType,
                    };

                default:
                    throw new ArgumentException("Could not resolve for this document.");
            }
        }

        private static string GetElementValueFromXml(IEnumerable<XElement> elements, string elementName)
        {
            return elements.Where(x => x.Name.LocalName == elementName).First().Value;
        }

        private static Status GetStatus(string statusName)
        {
            switch (statusName)
            {
                case StatusConstants.StatusNew:
                    return Status.New;

                case StatusConstants.StatusInProgress:
                    return Status.InProgress;

                case StatusConstants.StatusSuccess:
                    return Status.Success;

                case StatusConstants.StatusFailed:
                    return Status.Failed;

                default:
                    throw new ArgumentException("Invalid status");
            }
        }

        public override IQueryable<ICommand> GetAll()
        {
            return GetAllFromCollection(CommandConstants.CollectionName);
        }
    }
}