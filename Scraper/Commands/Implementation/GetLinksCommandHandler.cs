using MarklogicDataLayer.DatabaseConnectors;
using MarklogicDataLayer.DataStructs;
using OfferScraper.Commands.Interfaces;
using OfferScraper.LinkGatherers;
using OfferScraper.Repositories;

namespace OfferScraper.Commands.Implementation
{
    public class GetLinksCommandHandler : ICommandHandler<GetLinksCommand>
    {
        private readonly IDatabaseConnectionSettings _databaseConnectionSettings;
        private readonly IDataRepository<Link> _dataRepository;
        private readonly ILinkGatherer _linkGatherer;

        public void Handle(GetLinksCommand command)
        {
            var links = _linkGatherer.Gather();

            foreach (var link in links)
            {
                _dataRepository.Insert(link);
            }
        }
    }
}