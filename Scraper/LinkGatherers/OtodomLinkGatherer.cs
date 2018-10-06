using Common.Loggers;
using MarklogicDataLayer.DataStructs;
using MarklogicDataLayer.Repositories;
using OfferScraper.Utilities.Browsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

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
        private readonly ILogger _logger;

        public OtodomLinkGatherer(IBrowser browser, DatabaseUtilityRepository utilityRepository, ILogger logger)
        {
            _browser = browser;
            _utilityRepository = utilityRepository;
            _logger = logger;
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
                var offerLinks = articles.Select(x => x.GetAttributeValue("data-url", "")).Where(x => x.Contains("www.otodom.pl/oferta/")).ToList();
                foreach (var offerLink in offerLinks)
                {
                    var offerPage = _browser.GetPage(new Uri(offerLink));
                    var offerDateText = offerPage.Descendants().First(x => x.Name == "p" && x.InnerText.Contains("Data dodania")).InnerText.Split(':').Last();
                    var numberOfDays = Regex.Match(offerDateText, "\\d+").Value;
                    var offerDateTime = new DateTime();
                    if (!DateTime.TryParse(offerDateText, out offerDateTime))
                    {
                        offerDateTime = DateTime.Now.AddDays(int.Parse(numberOfDays) * -1);
                    }

                    if (dateOfLastScrapping != null && dateOfLastScrapping > offerDateTime)
                    {
                        UpdateDateOfLastScraping();
                        return links;
                    }

                    _logger.Log(LogType.Info, $"Added {offerLink} link from OtoDom.");

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