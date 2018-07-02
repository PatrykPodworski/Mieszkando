using System;
using System.Threading;
using OfferLinkScraper.Crawlers;

namespace OfferLinkScraper
{
    public class WebScrapper
    {
        private readonly IDatabaseConnectionSettings _databaseConnectionSettings;
        private readonly WebCrawlersList _webCrawlersList;

        public WebScrapper()
        {
            _webCrawlersList = new WebCrawlersList(new OlxServiceCrawler());
            _databaseConnectionSettings = new DatabaseConnectionSettings("mieszkando-db");
        }

        public void Run()
        {
            foreach (var webCrawler in _webCrawlersList.AllWebCrawlers)
            {
                webCrawler.GetLinks();
            }
        }
    }
}