using MarklogicDataLayer.DataStructs;
using OfferScraper.LinkGatherers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OfferScraper.Factories
{
    public class LinkGathererFactory : IFactory<ILinkGatherer>
    {
        private readonly IEnumerable<ILinkGatherer> _linkGaterers;

        public LinkGathererFactory(IEnumerable<ILinkGatherer> linkGatherers)
        {
            _linkGaterers = linkGatherers;
        }

        public ILinkGatherer Get(OfferType type)
        {
            var gatherer = _linkGaterers
                .FirstOrDefault(x => x.GetSupportedType() == type);

            return gatherer ?? throw new ArgumentException("There isn't any link gatherer supporting given offer type.");
        }
    }
}