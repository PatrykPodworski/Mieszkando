using MarklogicDataLayer.DataStructs;
using OfferScraper.Commands.Interfaces;
using OfferScraper.Factories;
using OfferScraper.LinkGatherers;
using OfferScraper.Repositories;
using OfferScraper.Utilities.Extensions;
using OfferScraper.Utilities.Loggers;
using System;

namespace OfferScraper.Commands.Implementation
{
    public class GetLinksCommandHandler : BaseCommand, ICommandHandler<GetLinksCommand>
    {
        private readonly IFactory<ILinkGatherer> _factory;
        private readonly ILogger _logger;
        private readonly IDataRepository<Link> _dataRepository;
        private readonly ICommandQueue _commandQueue;

        public GetLinksCommandHandler(IDataRepository<Link> linkRepository, ICommandQueue commandRepository, IFactory<ILinkGatherer> factory, ILogger logger)
        {
            _dataRepository = linkRepository;
            _commandQueue = commandRepository;
            _factory = factory;

            _logger = logger;
            _logger.SetSource(typeof(GetLinksCommandHandler).Name);
        }

        public void Handle(ICommand comm)
        {
            try
            {
                CheckCommandType(comm);
                var command = (GetLinksCommand)comm;

                var gatherer = _factory.Get(command.Type);
                var links = gatherer.Gather();

                _logger.Log(LogType.Info, $"{gatherer.GetClassName()} gathered {links.Count} new links");

                using (var transaction = _dataRepository.GetTransaction())
                {
                    _dataRepository.Insert(links, transaction);
                    AddGatherDataCommands(links.Count, transaction);
                }

                _logger.Log(LogType.Info, $"Inserted {links.Count} new links into database");
            }
            catch (Exception e)
            {
                _logger.Log(LogType.Error, $"Error while handling command {comm.GetClassName()} with id {comm.GetId()}|{e.Message}");
                throw;
            }
        }

        public void AddGatherDataCommands(int numberOfLinks, ITransaction transaction)
        {
            var linksPerCommand = 10;   // configuration

            for (int i = 0; i < numberOfLinks / linksPerCommand; i++)
            {
                _commandQueue.Add(new GatherDataCommand(linksPerCommand) { Id = Guid.NewGuid().ToString() }, transaction);
            }

            _commandQueue.Add(new GatherDataCommand(numberOfLinks % linksPerCommand) { Id = Guid.NewGuid().ToString() }, transaction);

            _logger.Log(LogType.Info, $"Created new {typeof(GatherDataCommand).Name}s: {numberOfLinks / linksPerCommand}x{linksPerCommand} and 1x{numberOfLinks % linksPerCommand}");
        }
    }
}