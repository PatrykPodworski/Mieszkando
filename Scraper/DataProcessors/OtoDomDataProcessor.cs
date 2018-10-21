using Common.Extensions;
using HtmlAgilityPack;
using MarklogicDataLayer.DataStructs;
using ScrapySharp.Extensions;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace OfferScraper.DataProcessors
{
    public class OtoDomDataProcessor : IDataProcessor
    {
        public Offer Process(HtmlData sample)
        {
            var offer = GetData(sample);

            return offer;
        }

        private Offer GetData(HtmlData htmlData)
        {
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(htmlData.Content);
            var data = htmlDocument.DocumentNode;

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
                ?.InnerHtml
                ?.GetNumber() ?? "0";

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

            var dateOfPostingText = data.CssSelect(".text-details > .right > p")
                .FirstOrDefault(x => x.InnerHtml.Contains("Data dodania"))
                .InnerHtml
                .Split(':')
                .LastOrDefault()
                .Trim();

            var latitude = data
                .Descendants()
                .FirstOrDefault(x => x.Name == "meta" && x.GetAttributeValue("itemprop", "") == "latitude")
                .GetAttributeValue("content", "");

            var longitude = data
                .Descendants()
                .FirstOrDefault(x => x.Name == "meta" && x.GetAttributeValue("itemprop", "") == "longitude")
                .GetAttributeValue("content", ""); ;

            var numberOfDays = Regex.Match(dateOfPostingText, "\\d+").Value;
            var offerDateTime = new DateTime();
            if (!DateTime.TryParse(dateOfPostingText, out offerDateTime))
            {
                offerDateTime = DateTime.Now.AddDays(int.Parse(numberOfDays) * -1);
            }

            var dateOfPosting = offerDateTime
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
                DateOfScraping = dateOfScraping,
                Latitude = latitude,
                Longitude = longitude,
                LinkId = htmlData.LinkId,
            };
        }
    }
}