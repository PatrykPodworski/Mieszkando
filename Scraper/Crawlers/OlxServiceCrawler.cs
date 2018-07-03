using System.Collections.Generic;
using System.Linq;
using OfferLinkScraper.Repositories;
using ScrapySharp.Network;
using System;
using ScrapySharp.Extensions;

namespace OfferLinkScraper.Crawlers
{
    public class OlxServiceCrawler : ServiceCrawler
    {
        private static string AdvertisementClassName => "a.marginright5.link.linkWithHash";
        private static string PageNumberBlockClassName => "block br3 brc8 large tdnone lheight24";
        private static string PageQuery => $"?page=";
        private static string BaseUri => "https://www.olx.pl/gdansk/q-mieszkanie/";

        public override IEnumerable<Link> GetLinks()
        {
            var links = new List<Link>();
            LinkCounter = LinkCounter == 1 ? LinkLocalFileRepository.GetMaxId() : LinkCounter;
            var browser = new ScrapingBrowser();
            var pagesCount = GetPagesCount(browser);

            for (var i = 1; i <= pagesCount; i++)
            {
                var pageQuery = i > 1 ? $"{PageQuery}{i}" : string.Empty;
                var page = browser.NavigateToPage(new Uri($"{BaseUri}{pageQuery}"));
                var aTags = page.Html.CssSelect(AdvertisementClassName);
                links.AddRange(aTags.Select(x => new Link((++LinkCounter).ToString(), x.Attributes["href"].Value)));
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
