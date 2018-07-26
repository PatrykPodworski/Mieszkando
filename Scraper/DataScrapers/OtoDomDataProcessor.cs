using HtmlAgilityPack;
using MarklogicDataLayer.DataStructs;
using OfferScraper.Extensions;
using ScrapySharp.Extensions;
using System;
using System.Linq;

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

        private Offer GetData(HtmlNode data)
        {
            var id = data.CssSelect(".text-details > .left > p")
                .FirstOrDefault(x => x.InnerHtml.Contains("Nr oferty"))
                .InnerHtml
                .GetNumber();

            var title = data.CssSelect("header > h1")
                .FirstOrDefault()
                .InnerHtml;

            var cost = data.CssSelect(".box-price-value")
                .FirstOrDefault()
                .InnerHtml
                .GetNumber();

            var bonusCost = data.CssSelect(".sub-list > li")
                .FirstOrDefault(x => x.InnerHtml.Contains("Czynsz"))
                .InnerHtml
                .GetNumber();

            var rooms = data.CssSelect(".main-list > li")
                .FirstOrDefault(x => x.InnerHtml.Contains("Liczba pokoi"))
                .InnerHtml
                .GetNumber();

            var area = data.CssSelect(".main-list > .param_m strong")
                .FirstOrDefault()
                .InnerHtml
                .GetNumber();

            var district = data.CssSelect(".address-links > a")
                .LastOrDefault()
                .InnerHtml;

            var dateOfPosting = data.CssSelect(".text-details > .right > p")
                .FirstOrDefault(x => x.InnerHtml.Contains("Data dodania"))
                .InnerHtml
                .Split(':')
                .LastOrDefault()
                .Trim()
                .ToShortDateString();

            var dateOfScraping = DateTime.Now
                .ToShortDateString();

            return new Offer
            {
                Id = id,
                Title = title,
                Cost = cost,
                BonusCost = bonusCost,
                Rooms = rooms,
                Area = area,
                District = district,
                DateOfPosting = dateOfPosting,
                DateOfScraping = dateOfScraping
            };
        }
    }
}