using System;

namespace OfferLinkScraper.Crawlers
{
    public class WebCrawlersList
    {
        public WebCrawlersList(params ServiceCrawler[] webServiceCrawlers)
        {
            foreach (var webServiceCrawler in webServiceCrawlers)
            {
                if (webServiceCrawler == null)
                    throw new ArgumentNullException(nameof(webServiceCrawler));
            }

            AllWebCrawlers = webServiceCrawlers;
        }

        public IWebServiceCrawler[] AllWebCrawlers { get; }
    }
}