using Common.Loggers;
using HtmlAgilityPack;
using MarklogicDataLayer.Constants;
using MarklogicDataLayer.DataStructs;
using MarklogicDataLayer.Repositories;
using OfferScraper.Utilities.Browsers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GarbageCollector.Activities
{
    public class OfferLinkCleanupActivity : IGCActivity
    {
        private IDataRepository<Offer> _databaseOfferRepository;
        private IBrowser _browser;
        private ILogger _logger;

        private const int _batchSize = 100;

        public OfferLinkCleanupActivity(IDataRepository<Offer> databaseOfferRepository, IBrowser browser, ILogger logger)
        {
            _databaseOfferRepository = databaseOfferRepository;
            _browser = browser;
            _logger = logger;
        }

        public GCActivityStatus Perform()
        {
            var result = GCActivityStatus.Started;
            var startFrom = 1;
            var offers = new List<Offer>();
            try
            {
                do
                {
                    offers = _databaseOfferRepository.GetFromCollection(OfferConstants.CollectionName, startFrom).ToList();
                    if (offers.Count == 0 && result == GCActivityStatus.Started)
                    {
                        result = GCActivityStatus.None;
                    }
                    foreach (var offer in offers)
                    {
                        _logger.Log(LogType.Info, $"Checking Offer with id: {offer.Id}");
                        var link = offer.Link;
                        var offerPage = _browser.GetPage(new Uri(link));
                        if (IsInactive(offerPage))
                        {
                            var updatedOffer = offer;
                            updatedOffer.Link = string.Empty;
                            updatedOffer.OfferType = OfferType.Outdated;
                            _logger.Log(LogType.Info, $"Offer with id: {offer.Id} is inactive");
                            _databaseOfferRepository.Update(updatedOffer);

                            result = GCActivityStatus.Performed;
                        }
                    }
                    startFrom += _batchSize;
                } while (offers.Count != 0);
            }
            catch (Exception)
            {
                result = GCActivityStatus.Error;
            }

            return result;
        }

        private bool IsInactive(HtmlNode offerPage)
        {
            return offerPage.InnerHtml.Contains("To ogłoszenie jest nieaktualne")
                || offerPage.InnerHtml.Contains("To ogłoszenie nie jest już dostępne");
        }
    }
}
