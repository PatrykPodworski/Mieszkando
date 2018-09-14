using MarklogicDataLayer.DataStructs;
using OfferScraper.LinkGatherers;
using OfferScraper.Repositories;
using System;

namespace OfferScraper.Factories
{
    public class LinkGathererFactory : IFactory<ILinkGatherer>
    {
        private DatabaseUtilityRepository _repository;

        public LinkGathererFactory(DatabaseUtilityRepository repository)
        {
            _repository = repository;
        }

        public ILinkGatherer Get(OfferType type)
        {
            switch (type)
            {
                case OfferType.Olx:
                    return new OlxLinkGatherer(_repository);

                case OfferType.OtoDom:
                    return new OtodomLinkGatherer(_repository);

                default:
                    throw new ArgumentException("Couldn't resolve dependency for given OfferType");
            }
        }
    }
}