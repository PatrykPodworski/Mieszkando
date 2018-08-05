using MarklogicDataLayer.DataStructs;
using System.Collections.Generic;

namespace OfferScraper.LinkGatherers
{
    public interface ILinkGatherer
    {
        ICollection<Link> Gather();
    }
}