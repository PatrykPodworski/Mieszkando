using HtmlAgilityPack;
using MarklogicDataLayer.DataStructs;

namespace OfferScraper.DataScrapers
{
    public class OtoDomDataProcessor : IDataProcessor
    {
        public Offer Process(HTMLData sample)
        {
            var data = new HtmlDocument();
            data.LoadHtml(sample.Content);

            var offer = GetData(data.DocumentNode);

            return offer;
        }

        private Offer GetData(HtmlNode documentNode)
        {
            return new Offer
            {
            };
        }
    }
}