using System;
using OfferLinkScraper.Crawlers;

namespace OfferLinkScraper.Crawlers
{
    public class WebCrawlersList
    {
        public WebCrawlersList(OlxServiceCrawler olxServiceCrawler)
        {
            if (olxServiceCrawler == null)
                throw new ArgumentNullException(nameof(olxServiceCrawler));

            AllWebCrawlers = new IWebServiceCrawler[] {olxServiceCrawler};
        }

        public IWebServiceCrawler[] AllWebCrawlers { get; }
    }
}