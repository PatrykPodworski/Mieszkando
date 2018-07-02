using System.Collections.Generic;
using System.Linq;

namespace OfferLinkScraper.Crawlers
{
    public class OlxServiceCrawler : ServiceCrawler
    {
        public int PageCounter { get; set; }

        protected override string BaseUri => "https://www.olx.pl/gdansk/q-mieszkanie/";
        protected override string AdvertisementClassName => "marginright5 link linkWithHash";

        public override List<string> GetLinks()
        {
            var result = new List<string>();
            var htmlDocument = GetHtmlDocFromUri();
            var adLinks = htmlDocument.DocumentNode.Descendants().Where(x => x.GetAttributeValue("class", "").Contains(AdvertisementClassName))
                .Select(x => x.GetAttributeValue("href", "")).Distinct().ToList();

            return result;
        }
    }
}
