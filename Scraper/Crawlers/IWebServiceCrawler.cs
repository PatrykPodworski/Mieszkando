using OfferScrapper.DataStructs;
using System.Collections.Generic;

namespace OfferScrapper.Crawlers
{
    public interface IWebServiceCrawler
    {
        IEnumerable<Link> GetLinks();
    }
}
