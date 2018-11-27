﻿using Common.Extensions;
using HtmlAgilityPack;
using MarklogicDataLayer.DataStructs;
using OfferScraper.Utilities.DataProcessors;
using ScrapySharp.Extensions;
using System;
using System.Globalization;
using System.Linq;

namespace OfferScraper.DataProcessors
{
    public class OlxDataProcessor : IDataProcessor
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

            var id = data.CssSelect(".offer-titlebox__details small")
                .FirstOrDefault(x => x.InnerHtml.Contains("ID"))
                ?.InnerHtml
                ?.Split(':')
                ?.LastOrDefault()
                ?.Trim() ?? Guid.NewGuid().ToString();

            var title = data.CssSelect(".offer-titlebox > h1")
                .FirstOrDefault()
                .InnerHtml
                .Trim();

            var cost = data.CssSelect(".price-label > strong")
                .FirstOrDefault()
                .InnerHtml
                .GetNumber();

            var bonusCost = data.CssSelect(".details .value > strong")
                .FirstOrDefault(x => x.InnerHtml.Contains("zł"))
                .InnerHtml
                .GetNumber();

            var rooms = OlxRoomParser.Parse(data.CssSelect(".details .value a")
                .FirstOrDefault(x => x.Attributes["href"].Value.Contains("filter_enum_rooms"))
                .Attributes["href"].Value
                .Split('=')
                .LastOrDefault());

            var area = data.CssSelect(".details .value > strong")
                .FirstOrDefault(x => x.InnerHtml.Contains("m²"))
                .InnerHtml
                .GetNumber();

            var district = data.CssSelect(".show-map-link > strong")
                .FirstOrDefault()
                .InnerHtml
                .Split(',')
                .LastOrDefault()
                .Trim();

            var dateFormat = "dd.MM.yyyy";

            var dateOfPosting = data.CssSelect(".offer-titlebox__details em")
                .First()
                .InnerHtml
                .Split(",")
                .Second()
                .ToString(dateFormat);

            var latitude = data.CssSelect("#mapcontainer")
                .First()
                .GetAttributeValue("data-lat", "");

            var longitude = data.CssSelect("#mapcontainer")
                .First()
                .GetAttributeValue("data-lon", "");

            var dateOfScraping = DateTime.Now
                .ToString(dateFormat);

            return new Offer
            {
                Id = id,
                Title = title,
                Cost = double.Parse(cost, CultureInfo.InvariantCulture),
                BonusCost = double.Parse(bonusCost, CultureInfo.InvariantCulture),
                Rooms = int.Parse(rooms),
                Area = double.Parse(area, CultureInfo.InvariantCulture),
                District = district,
                DateOfPosting = dateOfPosting,
                DateOfScraping = dateOfScraping,
                Latitude = double.Parse(latitude, CultureInfo.InvariantCulture),
                Longitude = double.Parse(longitude, CultureInfo.InvariantCulture),
                Link = htmlData.Link,
                TotalCost = (double.Parse(cost, System.Globalization.CultureInfo.InvariantCulture) + double.Parse(bonusCost, System.Globalization.CultureInfo.InvariantCulture)),
                OfferType = OfferType.Olx
            };
        }
    }
}