using MarklogicDataLayer.DataStructs;
using OfferScraper.Repositories;
using OfferScraper.Utilities.Browsers;
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

        private readonly IBrowser _browser;
        private readonly DatabaseUtilityRepository _utilityRepository;

        public OtodomLinkGatherer(IBrowser browser, DatabaseUtilityRepository utilityRepository)
        {
            _browser = browser;
            _utilityRepository = utilityRepository;
        }

        public ICollection<Link> Gather()
        {
            var links = new List<Link>();

            var pagesCount = GetPagesCount(_browser);
            var linksCount = 1;

            var dateOfLastScrapping = _utilityRepository.GetByKind(OfferType.OtoDom)?.DateOfLastScraping;
            
            for (var i = 1; i <= pagesCount; i++)
            {
                var pageQuery = i > 1 ? $"{PageQuery}{i}" : string.Empty;
                var page = _browser.GetPage(new Uri($"{BaseUri}{pageQuery}"));
                var articles = page.Descendants()
                    .Where(x => x.Name == AdvertisementElementName)
                    .Where(x => x.GetAttributeValue("data-featured-name", "") == AdvertisementClassName)
                    .ToList();
                var offerLinks = articles.Select(x => x.GetAttributeValue("data-url", "")).ToList();
                foreach (var offerLink in offerLinks)
                {
                    var offerPage = _browser.GetPage(new Uri(offerLink));
                    var offerDateText = offerPage.Descendants().First(x => x.Name == "p" && x.InnerText.Contains("Data aktualizacji")).InnerText;
                    var offerDate = offerDateText.Split(':').Last();
                    var offerDateTime = DateTime.Parse(offerDate);
                    if (dateOfLastScrapping != null && dateOfLastScrapping > offerDateTime)
                    {
                        UpdateDateOfLastScraping();
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

            UpdateDateOfLastScraping();
            return links;
        }

        private void UpdateDateOfLastScraping()
        {
            _utilityRepository.Insert(new Utility()
            {
                DateOfLastScraping = DateTime.Now,
                Type = OfferType.OtoDom,
            });
        }

        private static int GetPagesCount(IBrowser _browser)
        {
            var page = _browser.GetPage(new Uri($"{BaseUri}"));
            return int.Parse(page.Descendants().Where(x =>
                    x.GetAttributeValue("class", "").Contains(PageNumberBlockClassName))
                .SelectMany(x => x.ChildNodes.Where(y => y.Name == "strong")).Last()
                .InnerText);
        }
    }
}