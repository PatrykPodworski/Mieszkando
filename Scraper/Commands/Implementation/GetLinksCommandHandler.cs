using MarklogicDataLayer.DataStructs;
using OfferScraper.Commands.Interfaces;
using OfferScraper.Factories;
using OfferScraper.LinkGatherers;
using OfferScraper.Repositories;
using System;
using System.Linq;

namespace OfferScraper.Commands.Implementation
{
    public class GetLinksCommandHandler : ICommandHandler<GetLinksCommand>
    {
        private readonly IFactory<ILinkGatherer> _factory;
        private readonly IDataRepository<Link> _linkRepository;
        private readonly ICommandQueue _commandQueue;

        public GetLinksCommandHandler(IDataRepository<Link> linkRepository, ICommandQueue commandRepository, IFactory<ILinkGatherer> factory)
        {
            _linkRepository = linkRepository;
            _commandQueue = commandRepository;
            _factory = factory;
        }

        public void Handle(GetLinksCommand command)
        {
            var gatherer = _factory.Get(command.Type);

            var newestLink = _linkRepository
                .GetAll()
                .OrderByDescending(x => x.LastUpdate)
                .FirstOrDefault();

            var links = gatherer.Gather(newestLink);

            using (var transaction = _linkRepository.GetTransaction())
            {
                _linkRepository.Insert(links, transaction);
                _commandQueue.Add((new GatherDataCommand(links.Count)));
            }
        }

        public Type GetSupportedType()
        {
            return GetType().GetGenericArguments()[0];
        }
    }
}