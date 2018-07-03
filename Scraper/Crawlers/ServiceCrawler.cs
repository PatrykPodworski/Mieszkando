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

        public static int LinkCounter = 1; // probably bad practice - need to think of a better way to count globally

        public abstract IEnumerable<Link> GetLinks();
    }
}
