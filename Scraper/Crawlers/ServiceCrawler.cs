using MarklogicDataLayer.DataStructs;
using System.Collections.Generic;

namespace OfferScraper.Crawlers
{
    public abstract class ServiceCrawler : IWebServiceCrawler
    {
        public static int LinkCounter = 1; // probably bad practice - need to think of a better way to count globally

        public abstract IEnumerable<Link> GetLinks();
    }
}