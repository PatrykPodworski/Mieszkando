using MarklogicDataLayer.DataStructs;
using OfferScraper.Utility.Browsers;
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

        private readonly IBrowser _browser;

        public OtodomLinkGatherer(IBrowser browser)
        {
            _browser = browser;
        }

        public ICollection<Link> Gather(Link newestLink)
        {
            var links = new List<Link>();

            var pagesCount = GetPagesCount(_browser);

            for (var i = 1; i <= pagesCount; i++)
            {
                var pageQuery = i > 1 ? $"{PageQuery}{i}" : string.Empty;
                var page = _browser.GetPage(new Uri($"{BaseUri}{pageQuery}"));
                var aTags = page.Descendants().Where(x =>
                    x.GetAttributeValue("data-featured-tracking", "").Contains(AdvertisementClassName)).ToList();
                links.AddRange(aTags.Select(x => x.GetAttributeValue("href", "")).Distinct().Select(x => new Link
                {
                    Uri = x,
                    LinkSourceKind = OfferType.Otodom,
                    LastUpdate = DateTime.Now,
                    Status = Status.New,
                }));
            }

            return links;
        }

        private static int GetPagesCount(IBrowser browser)
        {
            var page = browser.GetPage(new Uri($"{BaseUri}"));
            return int.Parse(page.Descendants().Where(x =>
                    x.GetAttributeValue("class", "").Contains(PageNumberBlockClassName))
                .SelectMany(x => x.ChildNodes.Where(y => y.Name == "strong")).Last()
                .InnerText);
        }

        public OfferType GetSupportedType()
        {
            return OfferType.Otodom;
        }
    }
}