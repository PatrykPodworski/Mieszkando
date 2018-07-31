using MarklogicDataLayer.DataStructs;
using System.Collections.Generic;

namespace OfferScraper.LinkGatherers
{
    public interface ILinkGatherer
    {
        IEnumerable<Link> Gather();
    }
}