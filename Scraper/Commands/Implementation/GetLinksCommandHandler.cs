using MarklogicDataLayer.DataStructs;
using OfferScraper.Commands.Interfaces;
using OfferScraper.Factories;
using OfferScraper.LinkGatherers;
using OfferScraper.Repositories;
using System;

namespace OfferScraper.Commands.Implementation
{
    public class GetLinksCommandHandler : ICommandHandler<GetLinksCommand>
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

        public void Handle(GetLinksCommand command)
        {
            var gatherer = _factory.Get(command.Type);
            var links = gatherer.Gather();

            using (var transaction = _dataRepository.GetTransaction())
            {
                _dataRepository.Insert(links, transaction);
                _commandQueue.Add((new GatherDataCommand(links.Count)));
            }
        }

        public Type GetCommandType()
        {
            return GetType().GetGenericArguments()[0];
        }
    }
}