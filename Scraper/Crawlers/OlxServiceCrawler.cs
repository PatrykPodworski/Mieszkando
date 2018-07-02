using System.Collections.Generic;
using System.Linq;
using OfferLinkScraper.Repositories;

namespace OfferLinkScraper.Crawlers
{
    public class OlxServiceCrawler : ServiceCrawler
    {
        public int PageCounter { get; set; }

        protected override string BaseUri => "https://www.olx.pl/gdansk/q-mieszkanie/";
        protected override string AdvertisementClassName => "marginright5 link linkWithHash";

        public override IEnumerable<Link> GetLinks()
        {
            LinkCounter = LinkLocalFileRepository.GetMaxId();
            var htmlDocument = GetHtmlDocFromUri();
            var adLinks = htmlDocument.DocumentNode.Descendants().Where(x => x.GetAttributeValue("class", "").Contains(AdvertisementClassName))
                .Select(x => x.GetAttributeValue("href", "")).Distinct().Select(x => new Link((++LinkCounter).ToString(), x));

            return adLinks;
        }
    }
}
