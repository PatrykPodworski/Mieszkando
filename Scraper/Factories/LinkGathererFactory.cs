using MarklogicDataLayer.DataStructs;
using OfferScraper.LinkGatherers;
using OfferScraper.Repositories;
using OfferScraper.Utilities.Browsers;
using System;

namespace OfferScraper.Factories
{
    public class LinkGathererFactory : IFactory<ILinkGatherer>
    {
        private IBrowser _browser;
        private DatabaseUtilityRepository _repository;

        public LinkGathererFactory(IBrowser browser, DatabaseUtilityRepository repository)
        {
            _browser = browser;
            _repository = repository;
        }

        public ILinkGatherer Get(OfferType type)
        {
            switch (type)
            {
                case OfferType.Olx:
                    return new OlxLinkGatherer(_browser, _repository);

                case OfferType.OtoDom:
                    return new OtodomLinkGatherer(_browser, _repository);

                default:
                    throw new ArgumentException("Couldn't resolve dependency for given OfferType");
            }
        }
    }
}