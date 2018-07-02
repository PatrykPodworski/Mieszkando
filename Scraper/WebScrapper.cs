using OfferLinkScraper.Crawlers;
using OfferLinkScraper.DatabaseConnectors;
using OfferLinkScraper.Repositories;
using System.Collections.Generic;

namespace OfferLinkScraper
{
    public class WebScrapper
    {
        private readonly IDatabaseConnectionSettings _databaseConnectionSettings;
        private readonly WebCrawlersList _webCrawlersList;
        private readonly IDataRepository<Link> _dataRepository;

        public WebScrapper()
        {
            _webCrawlersList = new WebCrawlersList(new OlxServiceCrawler(), new OtodomServiceCrawler());
            _databaseConnectionSettings = new DatabaseConnectionSettings("mieszkando-db");
            _dataRepository = new LinkLocalFileRepository();
        }

        public void Run()
        {
            var links = new List<Link>();
            foreach (var webCrawler in _webCrawlersList.AllWebCrawlers)
            {
                links.AddRange(webCrawler.GetLinks());
            }

            links.ForEach(x => _dataRepository.Insert(x));
        }
    }
}