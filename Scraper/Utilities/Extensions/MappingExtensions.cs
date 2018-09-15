using HtmlAgilityPack;
using MarklogicDataLayer.DataStructs;
using System;

namespace OfferScraper.Utilities.Extensions
{
    public static class MappingExtensions
    {
        public static Link MapToLink(this HtmlNode node)
        {
            if (node.Attributes["href"].Value.Contains("olx"))
            {
                return new Link
                {
                    Id = Guid.NewGuid().ToString(),
                    Uri = node.Attributes["href"].Value,
                    LinkSourceKind = OfferType.Olx,
                    LastUpdate = DateTime.Now,
                    Status = Status.New
                };
            }
            return null;
        }
    }
}