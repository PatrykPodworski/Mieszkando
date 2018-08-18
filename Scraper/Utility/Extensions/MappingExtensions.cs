using HtmlAgilityPack;
using MarklogicDataLayer.DataStructs;
using System;

namespace OfferScraper.Utility.Extensions
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
                DateOfGather = DateTime.Now,
                Status = Status.New
            };
        }
    }
}