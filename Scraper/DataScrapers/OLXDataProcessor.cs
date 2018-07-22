using HtmlAgilityPack;
using MarklogicDataLayer.DataStructs;
using ScrapySharp.Extensions;
using System.Linq;

namespace OfferScraper.DataScrapers
{
    public class OlxDataProcessor : IDataProcessor
    {
        public Offer Process(HTMLData sample)
        {
            var data = new HtmlDocument();
            data.LoadHtml(sample.Content);

            var offer = GetData(data.DocumentNode);

            return offer;
        }

        private Offer GetData(HtmlNode data)
        {
            var title = data.CssSelect(".offer-titlebox > h1").First().InnerHtml.Trim();

            return new Offer { Title = title };
        }
    }
}