using HtmlAgilityPack;
using MarklogicDataLayer.DataStructs;
using OfferScraper.Repositories;
using OfferScraper.Utilities.Browsers;
using OfferScraper.Utilities.Extensions;
using ScrapySharp.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OfferScraper.LinkGatherers
{
    public class OlxLinkGatherer : ILinkGatherer
    {
        private static string AdvertisementClassName => "#offers_table .wrap";
        private static string BaseUri => "https://www.olx.pl/nieruchomosci/mieszkania/wynajem/gdansk/";
        private static string PageQuery => "?page=";
        private static string LinkClassName => "a.marginright5.link.linkWithHash";
        private static string PageNumberBlockClassName => ".block.br3.brc8.large.tdnone.lheight24";

        private readonly IBrowser _browser;
        private readonly DatabaseUtilityRepository _utilityRepository;

        public OlxLinkGatherer(IBrowser browser, DatabaseUtilityRepository utilityRepository)
        {
            _browser = browser;
            _utilityRepository = utilityRepository;
        }

        public ICollection<Link> Gather()
        {
            var links = GetLinks();

            UpdateDateOfLastScrapping();

            return links;
        }

        private List<Link> GetLinks()
        {
            var numberOfPages = GetMaxPage(_browser);

            var aTags = new List<HtmlNode>();

            for (var i = 1; i <= numberOfPages; i++)
            {
                var page = _browser.GetPage(new Uri($"{BaseUri}{PageQuery}{i}"));
                var aTagsFromPage = page.CssSelect(AdvertisementClassName);

                aTags.AddRange(aTagsFromPage);
            }

            var dateOfLastScrapping = _utilityRepository.GetByKind(OfferType.Olx)?.DateOfLastScraping ?? DateTime.MinValue;

            var links = aTags
                .Where(x => IsNewer(x, dateOfLastScrapping))
                .Select(x => x.CssSelect(LinkClassName)
                .FirstOrDefault()
                .MapToLink())
                .Where(x => x != null)
                .ToList();

            return links;
        }

        private static int GetMaxPage(IBrowser browser)
        {
            var page = browser.GetPage(new Uri(BaseUri));

            return page
                .CssSelect(PageNumberBlockClassName)
                .FirstOrDefault(x => x.Attributes["data-cy"].Value == "page-link-last")
                .CssSelect("span")
                .FirstOrDefault()
                .InnerHtml
                .ParseToInt();
        }

        private bool IsNewer(HtmlNode node, DateTime dateOfLastScrapping)
        {
            var dateOfPosting = node.CssSelect(".breadcrumb span")
                .ToList()
                .Last()
                .InnerHtml
                .Trim()
                .RemoveHtmlTag("i")
                .TranslateDate()
                .ParseToDateTime();

            return dateOfPosting > dateOfLastScrapping;
        }

        private void UpdateDateOfLastScrapping()
        {
            var utility = _utilityRepository.GetByKind(OfferType.Olx) ?? new Utility(OfferType.Olx);

            using (var transaction = _utilityRepository.GetTransaction())
            {
                _utilityRepository.Update(utility, transaction);
            }
        }
    }
}