using System;
using System.Collections.Generic;
using System.Linq;
using OfferLinkScraper.Repositories;
using ScrapySharp.Network;
using OfferLinkScraper.DataStructs;

namespace OfferLinkScraper.Crawlers
{
    public class OtodomServiceCrawler : ServiceCrawler
    {
        private static string BaseUri => "https://www.otodom.pl/wynajem/mieszkanie/gdansk/?search%5Bdist%5D=0&search%5Bsubregion_id%5D=439&search%5Bcity_id%5D=40";
        private static string PageNumberBlockClassName => "pager-counter";
        private static string PageQuery => $"&page=";
        private static string AdvertisementClassName => "listing_no_promo";

        public override IEnumerable<Link> GetLinks()
        {
            var links = new List<Link>();
            LinkCounter = LinkCounter == 1 ? LinkLocalFileRepository.GetMaxId() : LinkCounter;
            var browser = new ScrapingBrowser();

#if DEBUG
            var pagesCount = 1;
#else
            var pagesCount = GetPagesCount(browser);
#endif

            for (var i = 1; i <= pagesCount; i++)
            {
                var pageQuery = i > 1 ? $"{PageQuery}{i}" : string.Empty;
                var page = browser.NavigateToPage(new Uri($"{BaseUri}{pageQuery}"));
                var aTags = page.Html.Descendants().Where(x =>
                    x.GetAttributeValue("data-featured-tracking", "").Contains(AdvertisementClassName)).ToList();
                links.AddRange(aTags.Select(x => x.GetAttributeValue("href", "")).Distinct().Select(x => new Link((++LinkCounter).ToString(), x, LinkKind.OtoDom)));
            }

            return links;
        }

        private static int GetPagesCount(ScrapingBrowser browser)
        {
            var page = browser.NavigateToPage(new Uri($"{BaseUri}"));
            return int.Parse(page.Html.Descendants().Where(x =>
                    x.GetAttributeValue("class", "").Contains(PageNumberBlockClassName))
                .SelectMany(x => x.ChildNodes.Where(y => y.Name == "strong")).Last()
                .InnerText);
        }
    }
}
