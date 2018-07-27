using MarklogicDataLayer.DataStructs;
using OfferScraper.Repositories;
using OfferScraper.Utility;
using ScrapySharp.Extensions;
using ScrapySharp.Network;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OfferScraper.Crawlers
{
    public class OlxServiceCrawler : ServiceCrawler
    {
        private static string AdvertisementClassName => "a.marginright5.link.linkWithHash";
        private static string PageNumberBlockClassName => "block br3 brc8 large tdnone lheight24";
        private static string PageQuery => $"?page=";
        private static string BaseUri => "https://www.olx.pl/nieruchomosci/mieszkania/wynajem/gdansk/";

        public override IEnumerable<Link> GetLinks()
        {
            var links = new List<Link>();
            LinkCounter = LinkCounter == 1 ? LinkLocalFileRepository.GetMaxId() : LinkCounter;
            var browser = BrowserFactory.GetBrowser();

#if DEBUG
            var pagesCount = 1;
#else
            var pagesCount = GetPagesCount(browser);
#endif

            for (var i = 1; i <= pagesCount; i++)
            {
                var pageQuery = i > 1 ? $"{PageQuery}{i}" : string.Empty;
                var page = browser.NavigateToPage(new Uri($"{BaseUri}{pageQuery}"));
                var aTags = page.Html.CssSelect(AdvertisementClassName);
                links.AddRange(aTags.Select(x => new Link((++LinkCounter).ToString(), x.Attributes["href"].Value, OfferType.Olx)));
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