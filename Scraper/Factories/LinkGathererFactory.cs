using Common.Loggers;
using MarklogicDataLayer.DataStructs;
using MarklogicDataLayer.Repositories;
using OfferScraper.LinkGatherers;
using OfferScraper.Utilities.Browsers;
using System;

namespace OfferScraper.Factories
{
    public class LinkGathererFactory : IFactory<ILinkGatherer>
    {
        private IBrowser _browser;
        private DatabaseUtilityRepository _repository;
        private ILogger _logger;

        public LinkGathererFactory(IBrowser browser, DatabaseUtilityRepository repository, ILogger logger)
        {
            _browser = browser;
            _repository = repository;
            _logger = logger;
        }

        public ILinkGatherer Get(OfferType type)
        {
            switch (type)
            {
                case OfferType.Olx:
                    return new OlxLinkGatherer(_browser, _repository, _logger);

                case OfferType.OtoDom:
                    return new OtodomLinkGatherer(_browser, _repository, _logger);

                default:
                    throw new ArgumentException("Couldn't resolve dependency for given OfferType");
            }
        }
    }
}