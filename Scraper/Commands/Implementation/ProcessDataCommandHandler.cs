using MarklogicDataLayer.DataStructs;
using OfferScraper.Commands.Interfaces;
using OfferScraper.DataExtractors;
using OfferScraper.Factories;
using OfferScraper.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OfferScraper.Commands.Implementation
{
    public class ProcessDataCommandHandler : ICommandHandler<ExtractDataCommand>
    {
        private readonly IFactory<IDataProcessor> _factory;
        private IDataRepository<HtmlData> _htmlDataRepository;
        private IDataRepository<Offer> _offerRepository;

        public void Handle(ExtractDataCommand command)
        {
            var htmlSamples = GetSamples(command.NumberOfSamples);

            GetOffers(htmlSamples);
        }

        private IEnumerable<HtmlData> GetSamples(int numberOfSamples)
        {
            return _htmlDataRepository
                 .SearchFor(x => x.Status == Status.New)
                 .Take(numberOfSamples);
        }

        private void ChangeSamplesStatusToInProgress(IEnumerable<HtmlData> htmlSamples)
        {
            using (var transaction = _htmlDataRepository.GetTransaction())
            {
                foreach (var htmlData in htmlSamples)
                {
                    htmlData.Status = Status.InProgress;
                }
            }
        }

        private void GetOffers(IEnumerable<HtmlData> htmlSamples)
        {
            try
            {
                ChangeSamplesStatusToInProgress(htmlSamples);

                foreach (var htmlData in htmlSamples)
                {
                    ProcessOffer(htmlData);
                }
            }
            catch (Exception)
            {
                ChangeAbortedSamplesStatusToNew(htmlSamples);
            }
        }

        private void ProcessOffer(HtmlData htmlData)
        {
            try
            {
                var processor = _factory.Get(htmlData.OfferType);
                var offer = processor.Process(htmlData);

                using (var transaction = _offerRepository.GetTransaction())
                {
                    _offerRepository.Insert(offer, transaction);

                    htmlData.Status = Status.Success;
                    _htmlDataRepository.Update(htmlData, transaction);
                }
            }
            catch (Exception)
            {
                using (var transaction = _htmlDataRepository.GetTransaction())
                {
                    htmlData.Status = Status.Failed;
                    _htmlDataRepository.Update(htmlData, transaction);
                }
                throw;
            }
        }

        private void ChangeAbortedSamplesStatusToNew(IEnumerable<HtmlData> htmlSamples)
        {
            foreach (var htmlData in htmlSamples)
            {
                using (var transaction = _htmlDataRepository.GetTransaction())
                {
                    htmlData.Status = Status.New;
                    _htmlDataRepository.Update(htmlData, transaction);
                }
            }
        }

        public Type GetCommandType()
        {
            return GetType().GetGenericArguments()[0];
        }
    }
}