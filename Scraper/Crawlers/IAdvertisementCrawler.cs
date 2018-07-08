using System.Collections.Generic;
using OfferLinkScraper.DataStructs;

namespace OfferLinkScraper.Crawlers
{
    public interface IAdvertisementCrawler
    {
        List<Advertisement> GetAds(IEnumerable<Link> links);
    }
}
