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
        private static string AdvertisementClassName => "#offers_table a.marginright5.link.linkWithHash";
        private static string PageNumberBlockClassName => "block br3 brc8 large tdnone lheight24";
        private static string PageQuery => $"?page=";
        private static string _baseUri => "https://www.olx.pl/nieruchomosci/mieszkania/wynajem/gdansk/";

        private readonly IBrowser _browser;

        public OlxLinkGatherer(IBrowser browser)
        {
            _browser = browser;
        }

        public ICollection<Link> Gather(Link newestLink)
        {
            var page = _browser.GetPage(new Uri(_baseUri));
            var aTags = page.CssSelect(AdvertisementClassName);

            return aTags
                .Select(x => x.MapToLink())
                .ToList();
        }

        public OfferType GetSupportedType()
        {
            return OfferType.Olx;
        }
    }
}