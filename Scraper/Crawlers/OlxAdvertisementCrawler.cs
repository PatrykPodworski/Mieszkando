using System.Collections.Generic;
using OfferLinkScraper.DataStructs;
using ScrapySharp.Network;
using System;

namespace OfferLinkScraper.Crawlers
{
    public class OlxAdvertisementCrawler : IAdvertisementCrawler
    {
        public List<Advertisement> GetAds(IEnumerable<Link> links)
        {
            var result = new List<Advertisement>();
            var browser = new ScrapingBrowser();

            foreach (var link in links)
            {
                var page = browser.NavigateToPage(new Uri(link.Uri));
                // todo: create crawling logic to create Ad objects
            }
            return result;
        }
    }
}
