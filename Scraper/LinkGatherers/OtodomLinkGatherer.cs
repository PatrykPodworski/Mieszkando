using MarklogicDataLayer.DataStructs;
using OfferScraper.Utility;
using ScrapySharp.Network;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OfferScraper.LinkGatherers
{
    public class OtodomLinkGatherer : ILinkGatherer
    {
        private static string BaseUri => "https://www.otodom.pl/wynajem/mieszkanie/gdansk/?search%5Bdist%5D=0&search%5Bsubregion_id%5D=439&search%5Bcity_id%5D=40";
        private static string PageNumberBlockClassName => "pager-counter";
        private static string PageQuery => $"&page=";
        private static string AdvertisementClassName => "listing_no_promo";

        public IEnumerable<Link> Gather()
        {
            var links = new List<Link>();
            var browser = BrowserFactory.GetBrowser();

            var pagesCount = GetPagesCount(browser);

            for (var i = 1; i <= pagesCount; i++)
            {
                var pageQuery = i > 1 ? $"{PageQuery}{i}" : string.Empty;
                var page = browser.NavigateToPage(new Uri($"{BaseUri}{pageQuery}"));
                var aTags = page.Html.Descendants().Where(x =>
                    x.GetAttributeValue("data-featured-tracking", "").Contains(AdvertisementClassName)).ToList();
                links.AddRange(aTags.Select(x => x.GetAttributeValue("href", "")).Distinct().Select(x => new Link
                {
                    Uri = x,
                    LinkSourceKind = OfferType.OtoDom,
                    LastUpdate = DateTime.Now,
                    Status = Status.New,
                }));
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