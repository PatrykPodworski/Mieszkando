using HtmlAgilityPack;
using MarklogicDataLayer.DataStructs;
using System;

namespace OfferScraper.Utilities.Extensions
{
    public static class MappingExtensions
    {
        public static Link MapToLink(this HtmlNode node)
        {
            return new Link
            {
                Uri = node.Attributes["href"].Value,
                LinkSourceKind = OfferType.Olx,
                LastUpdate = DateTime.Now,
                Status = Status.New
            };
        }
    }
}