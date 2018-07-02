using System;
using System.Threading;
using OfferLinkScraper.Crawlers;

namespace OfferLinkScraper
{
    public class WebScrapper
    {
        private IDatabaseConnectionSettings _databaseConnectionSettings;
        private WebCrawlersList _webCrawlersList;

        public WebScrapper()
        {
            _webCrawlersList = new WebCrawlersList(new OlxServiceCrawler());
            _databaseConnectionSettings = new DatabaseConnectionSettings("mieszkando-db");
        }

        public void Run()
        {
            throw new NotImplementedException();
        }
    }
}