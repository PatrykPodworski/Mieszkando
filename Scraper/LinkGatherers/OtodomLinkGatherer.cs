using MarklogicDataLayer.DataStructs;
using OfferScraper.Repositories;
using OfferScraper.Utility;
using ScrapySharp.Network;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OfferScraper.LinkGatherers
{
    public class OtodomLinkGatherer : ILinkGatherer
    {
        private static string BaseUri => "https://www.otodom.pl/wynajem/mieszkanie/gdansk/?search%5Bdist%5D=0&search%5Bsubregion_id%5D=439&search%5Bcity_id%5D=40";
        private static string PageNumberBlockClassName => "pager-counter";
        private static string PageQuery => $"&page=";
        private static string AdvertisementClassName => "listing_no_promo";
        private static string AdvertisementElementName => "article";
        private readonly DatabaseUtilityRepository _utilityRepository;

        public OtodomLinkGatherer(DatabaseUtilityRepository utilityRepository)
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
            
            for (var i = 1; i <= pagesCount; i++)
            {
                var pageQuery = i > 1 ? $"{PageQuery}{i}" : string.Empty;
                var page = browser.NavigateToPage(new Uri($"{BaseUri}{pageQuery}"));
                var articles = page.Html.Descendants()
                    .Where(x => x.Name == AdvertisementElementName)
                    .Where(x => x.GetAttributeValue("data-featured-name", "") == AdvertisementClassName)
                    .ToList();
                var offerLinks = articles.Select(x => x.GetAttributeValue("data-url", "")).ToList();
                foreach (var offerLink in offerLinks)
                {
                    var offerPage = browser.NavigateToPage(new Uri(offerLink));
                    var offerDateText = offerPage.Html.Descendants().First(x => x.Name == "p" && x.InnerText.Contains("Data aktualizacji")).InnerText;
                    var offerDate = offerDateText.Split(':').Last();
                    var offerDateTime = DateTime.Parse(offerDate);
                    if (dateOfLastScrapping == null || dateOfLastScrapping > offerDateTime)
                    {
                        dateOfLastScrapping = DateTime.Now;
                        _utilityRepository.Insert(new MarklogicDataLayer.DataStructs.Utility()
                        {
                            DateOfLastScraping = dateOfLastScrapping.GetValueOrDefault(),
                            Type = OfferType.OtoDom,
                        });
                        return links;
                    }
                    links.Add(new Link
                    {
                        Id = linksCount++.ToString(),
                        Uri = offerLink,
                        LinkSourceKind = OfferType.OtoDom,
                        LastUpdate = DateTime.Now,
                        Status = Status.New,
                    });
                }
            }

            return links;
        }

        private static int GetPagesCount(ScrapingBrowser browser)
        {
            var page = browser.NavigateToPage(new Uri($"{BaseUri}"));
            return int.Parse(page.Html.Descendants().Where(x =>
                    x.GetAttributeValue("class", "").Contains(PageNumberBlockClassName))
                .SelectMany(x => x.ChildNodes.Where(y => y.Name == "strong")).Last()
                .InnerText);
        }
    }
}