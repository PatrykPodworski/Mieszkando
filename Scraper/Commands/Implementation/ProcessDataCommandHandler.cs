using MarklogicDataLayer.DataStructs;
using OfferScraper.Commands.Interfaces;
using OfferScraper.DataProcessors;
using OfferScraper.Factories;
using OfferScraper.Repositories;
using OfferScraper.Utilities.Extensions;
using OfferScraper.Utilities.Loggers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OfferScraper.Commands.Implementation
{
    public class ProcessDataCommandHandler : BaseCommand, ICommandHandler<ExtractDataCommand>
    {
        private readonly IFactory<IDataProcessor> _factory;
        private IDataRepository<HtmlData> _htmlDataRepository;
        private IDataRepository<Offer> _offerRepository;
        private ILogger _logger;

        public ProcessDataCommandHandler(IFactory<IDataProcessor> factory, IDataRepository<HtmlData> htmlRepository, IDataRepository<Offer> offerRepository, ILogger logger)
        {
            _factory = factory;
            _htmlDataRepository = htmlRepository;
            _offerRepository = offerRepository;
            _logger = logger;
            _logger.SetSource(typeof(ProcessDataCommandHandler).Name);
        }

        public void Handle(ICommand comm)
        {
            CheckCommandType(comm);
            var command = (ExtractDataCommand)comm;

            var htmlSamples = GetSamples(command.NumberOfSamples);
            _logger.Log(LogType.Info, $"Got {htmlSamples.Count}/{command.NumberOfSamples} samples from database");

            GetOffers(htmlSamples);
        }

        private ICollection<HtmlData> GetSamples(int numberOfSamples)
        {
            return _htmlDataRepository
                 .GetAll()
                 .Where(x => x.Status == Status.New)
                 .Take(numberOfSamples)
                .ToList();
        }

        private void GetOffers(IEnumerable<HtmlData> htmlSamples)
        {
            try
            {
                _logger.Log(LogType.Debug, $"Before changing samples state to in progress");
                ChangeSamplesStatusToInProgress(htmlSamples);
                _logger.Log(LogType.Debug, $"After changing samples state to in progress");

                foreach (var htmlData in htmlSamples)
                {
                    ProcessOffer(htmlData);
                }
            }
            catch (Exception)
            {
                ChangeUnprocessedSamplesStatusToNew(htmlSamples);
            }
        }

        private void ChangeSamplesStatusToInProgress(IEnumerable<HtmlData> htmlSamples)
        {
            using (var transaction = _htmlDataRepository.GetTransaction())
            {
                foreach (var htmlData in htmlSamples)
                {
                    htmlData.Status = Status.InProgress;
                    _htmlDataRepository.Update(htmlData, transaction);
                }
            }
        }

        private void ProcessOffer(HtmlData htmlData)
        {
            try
            {
                _logger.Log(LogType.Debug, $"Before getting processor");
                var processor = _factory.Get(htmlData.OfferType);
                _logger.Log(LogType.Debug, $"After getting processor {processor.GetClassName()}");

                _logger.Log(LogType.Info, $"Started to extract data from {htmlData.GetClassName()} with Id: {htmlData.Id}");
                var offer = processor.Process(htmlData);
                _logger.Log(LogType.Info, $"Finished to extract data from {htmlData.GetClassName()} with Id: {htmlData.Id}");

                using (var transaction = _offerRepository.GetTransaction())
                {
                    _offerRepository.Insert(offer, transaction);
                    _logger.Log(LogType.Info, $"Inserted new {offer.GetClassName()} to database");

                    htmlData.Status = Status.Success;
                    _htmlDataRepository.Update(htmlData, transaction);
                    _logger.Log(LogType.Info, $"Updated {htmlData.GetClassName()} with Id: {htmlData.Id}");
                }
            }
            catch (Exception e)
            {
                using (var transaction = _htmlDataRepository.GetTransaction())
                {
                    htmlData.Status = Status.Failed;
                    _htmlDataRepository.Update(htmlData, transaction);
                    _logger.Log(LogType.Error, $"Failed to extract data from {htmlData.GetClassName()} with Id: {htmlData.Id}|{e.Message}");
                }
                throw;
            }
        }

        private void ChangeUnprocessedSamplesStatusToNew(IEnumerable<HtmlData> htmlSamples)
        {
            foreach (var htmlData in htmlSamples)
            {
                if (htmlData.Status != Status.InProgress)
                {
                    continue;
                }

                using (var transaction = _htmlDataRepository.GetTransaction())
                {
                    htmlData.Status = Status.New;
                    _htmlDataRepository.Update(htmlData, transaction);
                    _logger.Log(LogType.Info, $"Extracting data from {htmlData.GetClassName()} with Id: {htmlData.Id} left unfinished, changed status to new again");
                }
            }
        }
    }
}