using MarklogicDataLayer.DataStructs;
using OfferScraper.Utility.Browsers;
using System;

namespace OfferScraper.DataGatherers
{
    public class DataGatherer : IDataGatherer
    {
        private readonly IBrowser _browser;

        public DataGatherer(IBrowser browser)
        {
            _browser = browser;
        }

        public HtmlData Gather(Link link)
        {
            var uri = new Uri(link.Uri);
            var page = _browser.GetPage(uri);

            return new HtmlData
            {
                Content = page.InnerHtml,
                OfferType = link.LinkSourceKind,
                Status = Status.New,
                LastUpdate = DateTime.Now,
            };
        }
    }
}