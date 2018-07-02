using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using HtmlAgilityPack;

namespace OfferLinkScraper.Crawlers
{
    public class OlxServiceCrawler : IWebServiceCrawler
    {
        public int PageCounter { get; set; }

        private string BaseUri => "https://www.olx.pl/gdansk/q-mieszkanie/";
        private string AdvertisementClassName => "marginright5 link linkWithHash";

        public List<string> GetLinks()
        {
            var result = new List<string>();
            var htmlDocument = GetHtmlDocFromUri();
            var adNodes = htmlDocument.DocumentNode.Descendants().Where(x => x.GetAttributeValue("class", "").Contains(AdvertisementClassName)).ToList();

            return result;
        }

        private HtmlDocument GetHtmlDocFromUri()
        {
            var request = WebRequest.Create(BaseUri);
            var response = request.GetResponse();
            var data = response.GetResponseStream();
            var htmlDocument = new HtmlDocument();
            using (var sr = new StreamReader(data ?? throw new InvalidOperationException()))
            {
                htmlDocument.LoadHtml(sr.ReadToEnd());
            }

            return htmlDocument;
        }
    }
}
