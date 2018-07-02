using System.Collections.Generic;

namespace OfferLinkScraper.Crawlers
{
    public interface IWebServiceCrawler
    {
        IEnumerable<Link> GetLinks();
        int PageCounter { get; set; }
    }
}
