using MarklogicDataLayer.DataStructs;
using OfferScraper.Utilities.Browsers;
using System;

namespace OfferScraper.DataGatherers
{
    public class DataGatherer : IDataGatherer
    {
        private IBrowser _browser;

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
                Id = Guid.NewGuid().ToString(),
                Content = page.InnerHtml,
                OfferType = link.LinkSourceKind,
                Status = Status.New,
                LastUpdate = DateTime.Now,
                Link = link.Uri,
            };
        }
    }
}