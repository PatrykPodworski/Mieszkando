using Common.Loggers;
using GarbageCollector.Activities;
using MarklogicDataLayer.DataStructs;
using MarklogicDataLayer.Repositories;
using OfferScraper.Utilities.Browsers;
using System;
using System.Collections.Generic;
using System.Text;

namespace GarbageCollector
{
    public class GarbageCollectorService
    {
        private List<IGCActivity> _activities { get; set; }
        private IDataRepository<Offer> _offerRepository;
        private IBrowser _browser;
        private ILogger _logger;

        public GarbageCollectorService(IDataRepository<Offer> offerRepository, IBrowser browser, ILogger logger)
        {
            _offerRepository = offerRepository;
            _browser = browser;
            _logger = logger;

            _activities = new List<IGCActivity>()
            {
                new OfferLinkCleanupActivity(_offerRepository, _browser, _logger),
            };
        }

        public void Run()
        {
            foreach (var activity in _activities)
            {
                _logger.Log(LogType.Info, $"Starting activity {activity.GetType()}");
                if (activity.Perform() == GCActivityStatus.Error)
                {
                    _logger.Log(LogType.Error, $"Error while performing activity {activity.GetType()}");
                }
            }
        }
    }
}
