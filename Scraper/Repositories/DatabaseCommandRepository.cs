using MarklogicDataLayer.XQuery;
using OfferScraper.Commands.Interfaces;
using System.Collections.Generic;
using System.Linq;
using MarklogicDataLayer.DatabaseConnectors;
using System;
using System.Globalization;
using System.Xml.Linq;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using MarklogicDataLayer;
using MarklogicDataLayer.Constants;
using MarklogicDataLayer.DataStructs;
using MarklogicDataLayer.XQuery.Functions;
using OfferScraper.Commands.Implementation;
using OfferScraper.Constants;

namespace OfferScraper.Repositories
{
    public class DatabaseCommandRepository : DataRepository<ICommand>
    {
        public DatabaseCommandRepository(IDatabaseConnectionSettings databaseConnectionSettings) : base(databaseConnectionSettings, ExtractCommandInfo)
        {
        }

        public override void Delete(ICommand entity)
        {
            var flwor = new Flwor();
            flwor.Let(new VariableName("doc"), new CtsSearch("/",
                new CtsAndQuery(
                    new CtsElementValueQuery(CommandConstants.CreationDate,
                        (entity as BaseCommand)?.DateOfCreation.ToString("yyyy-MM-ddTHH:mm:ss.fffffffzzz")),
                    new CtsElementValueQuery(CommandConstants.LastModified,
                        (entity as BaseCommand)?.LastModified.ToString("yyyy-MM-ddTHH:mm:ss.fffffffzzz")),
                    new CtsCollectionQuery(entity.GetType().ToString().Split(".").Last()))));
            flwor.Return(new XdmpNodeDelete(new VariableName("doc").Query));
            RestConnector.Submit(flwor.Query);
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

        private static ICommand ExtractCommandInfo(XDocument xml)
        {
            var elements = xml.Descendants().ToList();
            var creationDate = Convert.ToDateTime(GetElementValueFromXml(elements, CommandConstants.CreationDate));
            var lastModified = Convert.ToDateTime(GetElementValueFromXml(elements, CommandConstants.LastModified));
            var status = GetElementValueFromXml(elements, CommandConstants.Status);
            switch (xml.Root.Name.LocalName)
            {
                case CommandConstants.ExtractDataCommand:
                    var numberOfSamples = int.Parse(GetElementValueFromXml(elements, CommandConstants.NumberOfSamples));
                    return new ExtractDataCommand()
                    {
                        DateOfCreation = creationDate,
                        LastModified = lastModified,
                        Status = GetStatus(status),
                        NumberOfSamples = numberOfSamples,
                    };
                case CommandConstants.GatherDataCommand:
                    var numberOfLinks = int.Parse(GetElementValueFromXml(elements, CommandConstants.NumberOfLinks));
                    return new GatherDataCommand()
                    {
                        DateOfCreation = creationDate,
                        LastModified = lastModified,
                        Status = GetStatus(status),
                        NumberOfLinks = numberOfLinks,
                    };
                case CommandConstants.GetLinksCommand:
                    var offerType = GetElementValueFromXml(elements, CommandConstants.OfferType) == OfferTypeConstants.Olx ? OfferType.Olx : OfferType.OtoDom;
                    return new GetLinksCommand()
                    {
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
    }
}