using MarklogicDataLayer.DataStructs;
using OfferScraper.LinkGatherers;
using OfferScraper.NinjectModules;
using System;
using Ninject;
using OfferScraper.Repositories;

namespace OfferScraper.Factories
{
    public class LinkGathererFactory : IFactory<ILinkGatherer>
    {
        public ILinkGatherer Get(OfferType type)
        {
            var kernel = new StandardKernel(new WebScrapperModule());
            switch (type)
            {
                case OfferType.Olx:
                    return new OlxLinkGatherer(kernel.Get<DatabaseUtilityRepository>());

                case OfferType.OtoDom:
                    return new OtodomLinkGatherer(kernel.Get<DatabaseUtilityRepository>());

                default:
                    throw new ArgumentException("Couldn't resolve dependency for given OfferType");
            }
        }
    }
}