using System;
using System.Collections.Generic;
using System.Text;

namespace OfferLinkScraper.Crawlers
{
    public class OlxServiceCrawler : IWebServiceCrawler
    {
        private string BaseUri => "https://www.olx.pl/gdansk/q-mieszkanie/";
        private string AdvertisementClass => "marginright5 link linkWithHash";
        public int PageCounter { get; set; }

        public List<string> GetLinks()
        {
            throw new NotImplementedException();
        }
    }
}
