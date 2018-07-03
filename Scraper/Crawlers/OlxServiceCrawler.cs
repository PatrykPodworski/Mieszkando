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
        public int PageCounter { get; set; }

        private static string BaseUri => "https://www.olx.pl/gdansk/q-mieszkanie/";
        private static string AdvertisementClassName => "a.marginright5.link.linkWithHash";

        public override IEnumerable<Link> GetLinks()
        {
            LinkCounter = LinkLocalFileRepository.GetMaxId();
            var browser = new ScrapingBrowser();
            var page = browser.NavigateToPage(new Uri(BaseUri));
            var aTags = page.Html.CssSelect(AdvertisementClassName);
            var links = aTags.Select(x => new Link((++LinkCounter).ToString(), x.Attributes["href"].Value));

            return links;
        }
    }
}
