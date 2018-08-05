using MarklogicDataLayer.DataStructs;
using OfferScraper.LinkGatherers;
using System;

namespace OfferScraper.Factories
{
    public class LinkGathererFactory : IFactory<ILinkGatherer>
    {
        public ILinkGatherer Get(OfferType type)
        {
            switch (type)
            {
                case OfferType.Olx:
                    return new OlxLinkGatherer();

                case OfferType.OtoDom:
                    return new OtodomLinkGatherer();

                default:
                    throw new ArgumentException("Couldn't resolve dependency for given OfferType");
            }
        }
    }
}