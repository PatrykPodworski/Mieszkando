using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using HtmlAgilityPack;

namespace OfferLinkScraper.Crawlers
{
    public abstract class ServiceCrawler : IWebServiceCrawler
    {
        public int PageCounter { get; set; }

        protected abstract string BaseUri { get; }
        protected abstract string AdvertisementClassName { get; }

        public abstract List<string> GetLinks();

        protected HtmlDocument GetHtmlDocFromUri()
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
