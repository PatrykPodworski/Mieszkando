using OfferLinkScraper.Crawlers;
using OfferLinkScraper.DatabaseConnectors;
using OfferLinkScraper.DataStructs;
using OfferLinkScraper.Repositories;
using System.Collections.Generic;

namespace OfferLinkScraper
{
    public class WebScrapper
    {
        private readonly IDatabaseConnectionSettings _databaseConnectionSettings;
        private readonly List<(IWebServiceCrawler WebServiceCrawler, IAdvertisementCrawler AdCrawler)> _webCrawlersTupleList;
        private readonly IDataRepository<Link> _dataRepository;

        public WebScrapper()
        {
            _webCrawlersTupleList = new List<(IWebServiceCrawler WebServiceCrawler, IAdvertisementCrawler AdCrawler)>
            {
                (new OlxServiceCrawler(), new OlxAdvertisementCrawler()),
                (new OtodomServiceCrawler(), new OtodomAdvertisementCrawler()),
            };
            _databaseConnectionSettings = new DatabaseConnectionSettings("mieszkando-db");
            _dataRepository = new LinkLocalFileRepository();
        }

        public void Run()
        {
            var links = new List<Link>();
            foreach (var webCrawlerTuple in _webCrawlersTupleList)
            {
                var scrappedLinks = webCrawlerTuple.WebServiceCrawler.GetLinks();

                webCrawlerTuple.AdCrawler.GetAds(scrappedLinks);

                links.AddRange(scrappedLinks);
            }

            links.ForEach(x => _dataRepository.Insert(x));
        }
    }
}