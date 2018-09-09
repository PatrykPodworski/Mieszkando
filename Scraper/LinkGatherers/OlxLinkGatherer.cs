using MarklogicDataLayer.DataStructs;
using OfferScraper.Utility;
using ScrapySharp.Extensions;
using ScrapySharp.Network;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OfferScraper.LinkGatherers
{
    public class OlxLinkGatherer : ILinkGatherer
    {
        private static string AdvertisementClassName => "a.marginright5.link.linkWithHash";
        private static string PageNumberBlockClassName => "block br3 brc8 large tdnone lheight24";
        private static string PageQuery => $"?page=";
        private static string BaseUri => "https://www.olx.pl/nieruchomosci/mieszkania/wynajem/gdansk/";

        public ICollection<Link> Gather()
        {
            var links = new List<Link>();
            var browser = BrowserFactory.GetBrowser();

            var pagesCount = GetPagesCount(browser);
            var linksCount = 1;

            for (var i = 1; i <= pagesCount; i++)
            {
                var pageQuery = i > 1 ? $"{PageQuery}{i}" : string.Empty;
                var page = browser.NavigateToPage(new Uri($"{BaseUri}{pageQuery}"));
                var aTags = page.Html.CssSelect(AdvertisementClassName);
                links.AddRange(aTags.Select(x => new Link
                {
                    Id = linksCount++.ToString(),
                    Uri = x.Attributes["href"].Value,
                    LinkSourceKind = OfferType.Olx,
                    LastUpdate = DateTime.Now,
                    Status = Status.New
                }));
            }

            return links;
        }

        private static int GetPagesCount(ScrapingBrowser browser)
        {
            var page = browser.NavigateToPage(new Uri($"{BaseUri}"));
            return int.Parse(page.Html.Descendants().Where(x =>
                    x.GetAttributeValue("class", "").Contains(PageNumberBlockClassName))
                .SelectMany(x => x.ChildNodes.Where(y => y.Name == "span")).Last()
                .InnerText);
        }
    }
}