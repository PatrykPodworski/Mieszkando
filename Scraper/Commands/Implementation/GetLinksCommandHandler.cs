using MarklogicDataLayer.DataStructs;
using OfferScraper.Commands.Interfaces;
using OfferScraper.Factories;
using OfferScraper.LinkGatherers;
using OfferScraper.Repositories;

namespace OfferScraper.Commands.Implementation
{
    public class GetLinksCommandHandler : BaseCommand, ICommandHandler<GetLinksCommand>
    {
        private readonly IFactory<ILinkGatherer> _factory;
        private readonly IDataRepository<Link> _dataRepository;
        private readonly ICommandQueue _commandQueue;

        public GetLinksCommandHandler(IDataRepository<Link> linkRepository, ICommandQueue commandRepository, IFactory<ILinkGatherer> factory)
        {
            _dataRepository = linkRepository;
            _commandQueue = commandRepository;
            _factory = factory;
        }

        public void Handle(ICommand comm)
        {
            CheckCommandType(comm);

            var command = (GetLinksCommand)comm;

            var gatherer = _factory.Get(command.Type);
            var links = gatherer.Gather();

            using (var transaction = _dataRepository.GetTransaction())
            {
                _dataRepository.Insert(links, transaction);
                AddGatherDataCommands(links.Count, transaction);
            }
        }

        public void AddGatherDataCommands(int numberOfLinks, ITransaction transaction)
        {
            var linksPerCommand = 10;   // configuration

            for (int i = 0; i < numberOfLinks; i += linksPerCommand)
            {
                _commandQueue.Add(new GatherDataCommand(linksPerCommand), transaction);
            }

            _commandQueue.Add(new GatherDataCommand(numberOfLinks % linksPerCommand), transaction);
        }
    }
}