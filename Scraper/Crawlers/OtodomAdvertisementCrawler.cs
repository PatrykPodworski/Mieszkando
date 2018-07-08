using System.Collections.Generic;
using OfferLinkScraper.DataStructs;

namespace OfferLinkScraper.Crawlers
{
    public class OtodomAdvertisementCrawler : IAdvertisementCrawler
    {
        public List<Advertisement> GetAds(IEnumerable<Link> links)
        {
            var result = new List<Advertisement>();
            foreach (var link in links)
            {

            }
            return result;
        }
    }
}
