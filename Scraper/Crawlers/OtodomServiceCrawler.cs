using System;
using System.Collections.Generic;
using System.Linq;
using OfferLinkScraper.Repositories;
using ScrapySharp.Network;

namespace OfferLinkScraper.Crawlers
{
    public class OtodomServiceCrawler : ServiceCrawler
    {
        private static string BaseUri => "https://www.otodom.pl/wynajem/mieszkanie/gdansk/?search%5Bdist%5D=0&search%5Bsubregion_id%5D=439&search%5Bcity_id%5D=40";
        private static string AdvertisementClassName => "listing_no_promo";

        public override IEnumerable<Link> GetLinks()
        {
            LinkCounter = LinkCounter == 1 ? LinkLocalFileRepository.GetMaxId() : LinkCounter;
            var browser = new ScrapingBrowser();
            var page = browser.NavigateToPage(new Uri(BaseUri));
            var aTags = page.Html.Descendants().Where(x =>
                x.GetAttributeValue("data-featured-tracking", "").Contains(AdvertisementClassName)).ToList();
            var links = aTags.Select(x => x.GetAttributeValue("href", "")).Distinct().Select(x => new Link((++LinkCounter).ToString(), x));

            return links;
        }
    }
}
