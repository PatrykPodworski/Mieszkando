using MarklogicDataLayer.DataProviders;
using MarklogicDataLayer.DataStructs;
using OfferScraper.Utility;
using System;

namespace OfferScraper.DataGatherers
{
    public class DataGatherer : IDataGatherer
    {
        private IDataProvider _dataProvider;

        public DataGatherer(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public void Gather(int numberOfLinks)
        {
            var links = _dataProvider.GetLinks(numberOfLinks);

            foreach (var link in links)
            {
                var data = GetHtml(link);

                _dataProvider.MarkAsGathered(link);
                _dataProvider.Save(data);
            }

            _dataProvider.Commit();
        }

        private HtmlData GetHtml(Link link)
        {
            var browser = BrowserFactory.GetBrowser();
            var uri = new Uri(link.Uri);
            var page = browser.NavigateToPage(uri);

            return new HtmlData
            {
                Content = page.Html.InnerHtml,
                OfferType = link.LinkSourceKind,
                IsProcessed = false
            };
        }
    }
}