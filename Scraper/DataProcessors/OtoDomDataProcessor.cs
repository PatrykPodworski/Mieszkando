﻿using Common.Extensions;
using HtmlAgilityPack;
using MarklogicDataLayer.DataStructs;
using ScrapySharp.Extensions;
using System;
using System.Globalization;
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
            if (!DateTime.TryParse(dateOfPostingText, new CultureInfo("pl-PL"), DateTimeStyles.None, out offerDateTime))
            {
                offerDateTime = DateTime.Now.AddDays(int.Parse(numberOfDays, CultureInfo.InvariantCulture) * -1);
            }

            var dateFormat = "dd.MM.yyyy";

            var dateOfPosting = offerDateTime.ToString(dateFormat);

            var dateOfScraping = DateTime.Now.ToString(dateFormat);

            return new Offer
            {
                Id = id,
                Title = title,
                Cost = double.Parse(cost.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture),
                BonusCost = double.Parse(bonusCost.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture),
                Rooms = int.Parse(rooms, NumberStyles.Any, CultureInfo.InvariantCulture),
                Area = double.Parse(area.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture),
                District = district,
                DateOfPosting = dateOfPosting,
                DateOfScraping = dateOfScraping,
                Latitude = double.Parse(latitude.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture),
                Longitude = double.Parse(longitude.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture),
                Link = htmlData.Link,
                TotalCost = (double.Parse(cost, NumberStyles.Any, CultureInfo.InvariantCulture) + double.Parse(bonusCost, NumberStyles.Any, CultureInfo.InvariantCulture)),
                OfferType = OfferType.OtoDom,
            };
        }
    }
}