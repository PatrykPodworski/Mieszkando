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
            var title = data.CssSelect(".offer-titlebox > h1")
                .FirstOrDefault()
                .InnerHtml
                .Trim();

            var cost = new string(data.CssSelect(".price-label > strong")
                .FirstOrDefault()
                .InnerHtml
                .Where(char.IsDigit)
                .ToArray());

            var rooms = data.CssSelect(".details .value a")
                .FirstOrDefault(x => x.Attributes["href"].Value.Contains("filter_enum_rooms"))
                .Attributes["href"].Value
                .Split('=')
                .Last();

            return new Offer
            {
                Title = title,
                Cost = cost,
                Rooms = rooms
            };
        }
    }
}