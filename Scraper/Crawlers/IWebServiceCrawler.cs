using MarklogicDataLayer.DataStructs;
using System.Collections.Generic;

namespace OfferScraper.Crawlers
{
    public interface IWebServiceCrawler
    {
        IEnumerable<Link> GetLinks();
    }
}