using MarklogicDataLayer.DataStructs;
using OfferScraper.Utility;
using System;

namespace OfferScraper.DataGatherers
{
    public class DataGatherer : IDataGatherer
    {
        public HtmlData Gather(Link link)
        {
            var browser = BrowserFactory.GetBrowser();
            var uri = new Uri(link.Uri);
            var page = browser.NavigateToPage(uri);

            return new HtmlData
            {
                Id = Guid.NewGuid().ToString(),
                Content = page.Html.InnerHtml,
                OfferType = link.LinkSourceKind,
                Status = Status.New,
                LastUpdate = DateTime.Now,
            };
        }
    }
}