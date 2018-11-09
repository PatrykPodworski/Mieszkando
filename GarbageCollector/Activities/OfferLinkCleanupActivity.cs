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
        private IDataRepository<Link> _databaseLinkRepository;
        private IBrowser _browser;
        private ILogger _logger;

        private const int _batchSize = 100;

        public OfferLinkCleanupActivity(IDataRepository<Offer> databaseOfferRepository, IDataRepository<Link> databaseLinkRepository, IBrowser browser, ILogger logger)
        {
            _databaseLinkRepository = databaseLinkRepository;
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
                    foreach (var offer in offers)
                    {
                        _logger.Log(LogType.Info, $"Checking Offer with id: {offer.Id}");
                        var linkId = offer.LinkId;
                        var linkDocument = _databaseLinkRepository.Get(LinkConstants.LinkId, linkId, LinkConstants.CollectionName, 1).First();
                        var link = linkDocument.Uri;
                        var offerPage = _browser.GetPage(new Uri(link));
                        if (IsInactive(offerPage))
                        {
                            var updatedOffer = offer;
                            updatedOffer.LinkId = string.Empty;
                            _logger.Log(LogType.Info, $"Offer with id: {offer.Id} is inactive");
                            _logger.Log(LogType.Info, $"Removing inactive Link with id: {linkDocument.Id}");
                            _databaseLinkRepository.Delete(linkDocument);
                            _databaseOfferRepository.Update(updatedOffer);
                        }
                    }
                    startFrom += _batchSize;
                } while (offers.Count != 0);

                result = GCActivityStatus.Performed;
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
