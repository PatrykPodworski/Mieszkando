using HtmlAgilityPack;
using MarklogicDataLayer.DataStructs;
using OfferScraper.Extensions;
using ScrapySharp.Extensions;
using System;
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
            var id = data.CssSelect(".offer-titlebox__details small")
                .FirstOrDefault(x => x.InnerHtml.Contains("ID"))
                .InnerHtml
                .Split(':')
                .Last()
                .Trim();

            var title = data.CssSelect(".offer-titlebox > h1")
                .FirstOrDefault()
                .InnerHtml
                .Trim();

            var cost = data.CssSelect(".price-label > strong")
                .FirstOrDefault()
                .InnerHtml
                .Where(char.IsDigit)
                .ToArray()
                .ParseToString();

            var rooms = data.CssSelect(".details .value a")
                .FirstOrDefault(x => x.Attributes["href"].Value.Contains("filter_enum_rooms"))
                .Attributes["href"].Value
                .Split('=')
                .Last();

            var area = data.CssSelect(".details .value > strong")
                .FirstOrDefault(x => x.InnerHtml.Contains("m²"))
                .InnerHtml
                .Where(char.IsDigit)
                .ToArray()
                .ParseToString();

            var district = data.CssSelect(".show-map-link > strong")
                .FirstOrDefault()
                .InnerHtml
                .Split(',')
                .Last()
                .Trim();

            var dateOfPosting = data.CssSelect(".offer-titlebox__details em")
                .First()
                .InnerHtml
                .Split(",")
                .Second()
                .ToShortDateString();

            var dateOfScraping = DateTime.Now
                .ToShortDateString();

            return new Offer
            {
                Id = id,
                Title = title,
                Cost = cost,
                Rooms = rooms,
                Area = area,
                District = district,
                DateOfPosting = dateOfPosting,
                DateOfScraping = dateOfScraping
            };
        }
    }
}