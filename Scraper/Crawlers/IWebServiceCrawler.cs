using System;
using System.Collections.Generic;
using System.Text;

namespace OfferLinkScraper.Crawlers
{
    public interface IWebServiceCrawler
    {
        List<string> GetLinks();
        int PageCounter { get; set; }
    }
}
