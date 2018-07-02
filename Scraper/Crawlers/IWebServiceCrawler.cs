using System.Collections.Generic;

namespace OfferLinkScraper.Crawlers
{
    public interface IWebServiceCrawler
    {
        List<string> GetLinks();
        int PageCounter { get; set; }
    }
}
