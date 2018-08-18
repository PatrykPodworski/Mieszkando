using MarklogicDataLayer.DataStructs;
using System.Collections.Generic;

namespace OfferScraper.LinkGatherers
{
    public interface ILinkGatherer
    {
        IList<Link> Gather(Link newestLink);

        OfferType GetSupportedType();
    }
}