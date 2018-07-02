using System.Collections.Generic;
using System.Linq;

namespace OfferLinkScraper.Crawlers
{
    public class OtodomServiceCrawler : ServiceCrawler
    {
        public int PageCounter { get; set; }

        protected override string BaseUri => "https://www.otodom.pl/wynajem/mieszkanie/gdansk/?search%5Bdist%5D=0&search%5Bsubregion_id%5D=439&search%5Bcity_id%5D=40";
        protected override string AdvertisementClassName => "listing_no_promo";

        public override List<string> GetLinks()
        {
            var result = new List<string>();
            var htmlDocument = GetHtmlDocFromUri();
            var adLinks = htmlDocument.DocumentNode.Descendants().Where(x => x.GetAttributeValue("data-featured-tracking", "").Contains(AdvertisementClassName))
                .Select(x => x.GetAttributeValue("href", "")).Distinct().ToList();

            return result;
        }
    }
}
