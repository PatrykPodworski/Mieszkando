﻿using MarklogicDataLayer;
using OfferLinkScraper.DatabaseConnectors;
using OfferScrapper.Crawlers;
using OfferScrapper.DataStructs;
using OfferScrapper.Repositories;
using System.Collections.Generic;

namespace OfferLinkScraper
{
    public class WebScrapper
    {
        private readonly IDatabaseConnectionSettings _databaseConnectionSettings;
        private readonly IWebServiceCrawler[] _webCrawlersArray;
        private readonly IDataRepository<Link> _dataRepository;

        public WebScrapper()
        {
            _webCrawlersArray = new IWebServiceCrawler[] { new OlxServiceCrawler(), new OtodomServiceCrawler() };
            _databaseConnectionSettings = new DatabaseConnectionSettings("mieszkando-db");
            _dataRepository = new LinkLocalFileRepository();
        }

        public void Run()
        {
            var links = new List<Link>();
            foreach (var webCrawler in _webCrawlersArray)
            {
                links.AddRange(webCrawler.GetLinks());
            }

            links.ForEach(x => _dataRepository.Insert(x));
        }
    }
}