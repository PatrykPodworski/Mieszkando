using MarklogicDataLayer.DataStructs;
using OfferScraper.Repositories;
using OfferScraper.Utility;
using ScrapySharp.Extensions;
using ScrapySharp.Network;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OfferScraper.LinkGatherers
{
    public class OlxLinkGatherer : ILinkGatherer
    {
        private static string AdvertisementClassName => "a.marginright5.link.linkWithHash";
        private static string PageNumberBlockClassName => "block br3 brc8 large tdnone lheight24";
        private static string PageQuery => $"?page=";
        private static string BaseUri => "https://www.olx.pl/nieruchomosci/mieszkania/wynajem/gdansk/";
        private readonly DatabaseUtilityRepository _utilityRepository;

        public OlxLinkGatherer(DatabaseUtilityRepository utilityRepository)
        {
            _utilityRepository = utilityRepository;
        }

        public ICollection<Link> Gather()
        {
            var links = new List<Link>();
            var browser = BrowserFactory.GetBrowser();

            var pagesCount = GetPagesCount(browser);
            var linksCount = 1;

            var dateOfLastScrapping = _utilityRepository.GetByKind(OfferType.OtoDom)?.DateOfLastScraping;

            dateOfLastScrapping = dateOfLastScrapping ?? DateTime.Now;

            for (var i = 1; i <= pagesCount; i++)
            {
                var pageQuery = i > 1 ? $"{PageQuery}{i}" : string.Empty;
                var page = browser.NavigateToPage(new Uri($"{BaseUri}{pageQuery}"));
                var aTags = page.Html.CssSelect(AdvertisementClassName);
                var newestDate = page.Html.Descendants()
                    .Where(x => x.Name == "i" && x.GetAttributeValue("data-icon") == "clock")
                    .Where(x => x.ParentNode.Name == "span")
                    .Select(x => x.ParentNode.InnerText.Trim())
                    .Where(x => x.Contains("dzisiaj"))
                    .Select(x => x.Split(' ').Last())
                    .Select(x => DateTime.Parse(x))
                    .ToList()
                    .Max();
                if (dateOfLastScrapping > newestDate)
                {
                    dateOfLastScrapping = DateTime.Now;
                    _utilityRepository.Insert(new MarklogicDataLayer.DataStructs.Utility()
                    {
                        DateOfLastScraping = dateOfLastScrapping.GetValueOrDefault(),
                        Type = OfferType.OtoDom,
                    });
                    return links;
                }
                links.AddRange(aTags.Select(x => new Link
                {
                    Id = linksCount++.ToString(),
                    Uri = x.Attributes["href"].Value,
                    LinkSourceKind = OfferType.Olx,
                    LastUpdate = DateTime.Now,
                    Status = Status.New
                }));
            }

            return links;
        }

        private static int GetPagesCount(ScrapingBrowser browser)
        {
            var page = browser.NavigateToPage(new Uri($"{BaseUri}"));
            return int.Parse(page.Html.Descendants().Where(x =>
                    x.GetAttributeValue("class", "").Contains(PageNumberBlockClassName))
                .SelectMany(x => x.ChildNodes.Where(y => y.Name == "span")).Last()
                .InnerText);
        }
    }
}