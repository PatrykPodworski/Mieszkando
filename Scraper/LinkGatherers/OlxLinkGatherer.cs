using HtmlAgilityPack;
using MarklogicDataLayer.DataStructs;
using OfferScraper.Utility.Browsers;
using OfferScraper.Utility.Extensions;
using ScrapySharp.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OfferScraper.LinkGatherers
{
    public class OlxLinkGatherer : ILinkGatherer
    {
        private static string AdvertisementClassName => "#offers_table .wrap";
        private static string LinkClassName => "a.marginright5.link.linkWithHash";
        private static string PageNumberBlockClassName => "block br3 brc8 large tdnone lheight24";
        private static string PageQuery => $"?page=";
        private static string _baseUri => "https://www.olx.pl/nieruchomosci/mieszkania/wynajem/gdansk/";

        private readonly IBrowser _browser;

        public OlxLinkGatherer(IBrowser browser)
        {
            _browser = browser;
        }

        public IList<Link> Gather(Link newestLink)
        {
            var page = _browser.GetPage(new Uri(_baseUri));
            var aTags = page.CssSelect(AdvertisementClassName);

            var newerLinks = aTags
                .Where(x => IsNewer(x, newestLink))
                .Select(x => x.CssSelect(LinkClassName)
                .FirstOrDefault()
                .MapToLink())
                .ToList();

            return newerLinks;
        }

        private bool IsNewer(HtmlNode node, Link link)
        {
            var dateOfPosting = node.CssSelect(".breadcrumb span")
                .ToList()
                .Last()
                .InnerHtml
                .Trim()
                .RemoveHtmlTag("i")
                .TranslateDate()
                .ParseToDateTime();

            return dateOfPosting > link.DateOfGather;
        }

        public OfferType GetSupportedType()
        {
            return OfferType.Olx;
        }
    }
}