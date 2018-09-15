using MarklogicDataLayer.DataStructs;
using OfferScraper.Commands.Interfaces;
using OfferScraper.DataGatherers;
using OfferScraper.Repositories;
using OfferScraper.Utilities.Extensions;
using OfferScraper.Utilities.Loggers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OfferScraper.Commands.Implementation
{
    public class GatherDataCommandHandler : BaseCommand, ICommandHandler<GatherDataCommand>
    {
        private readonly IDataGatherer _dataGatherer;
        private readonly ILogger _logger;
        private readonly ICommandQueue _commandQueue;
        private readonly IDataRepository<Link> _linkRepository;
        private readonly IDataRepository<HtmlData> _htmlDataRepository;

        public GatherDataCommandHandler(IDataRepository<Link> linkRepository, IDataRepository<HtmlData> htmlDataRepository,
            ICommandQueue commandQueue, IDataGatherer dataGatherer, ILogger logger)
        {
            _linkRepository = linkRepository;
            _htmlDataRepository = htmlDataRepository;
            _commandQueue = commandQueue;
            _dataGatherer = dataGatherer;
            _logger = logger;
            _logger.SetSource(typeof(GatherDataCommand).Name);
        }

        public void Handle(ICommand comm)
        {
            CheckCommandType(comm);
            var command = (GatherDataCommand)comm;

            var links = GetLinks(command.NumberOfLinks);
            _logger.Log(LogType.Info, $"Got {links.Count}/{command.NumberOfLinks} links from database");

            GetHtmlSamples(links);
        }

        private ICollection<Link> GetLinks(int numberOfLinks)
        {
            return _linkRepository
                    .GetAll()
                    .Where(x => x.Status == Status.New)
                    .Take(numberOfLinks)
                    .ToList();
        }

        private void GetHtmlSamples(ICollection<Link> links)
        {
            try
            {
                ChangeLinksStatusToInProgress(links);

                foreach (var link in links)
                {
                    GetHtml(link);
                }
            }
            catch (Exception)
            {
                ChangeUnprocessedLinksStatusToNew(links);
                throw;
            }
        }

        private void ChangeLinksStatusToInProgress(ICollection<Link> links)
        {
            using (var transaction = _linkRepository.GetTransaction())
            {
                foreach (var link in links)
                {
                    link.Status = Status.InProgress;
                    _linkRepository.Update(link, transaction);
                }
            }
        }

        private void GetHtml(Link link)
        {
            try
            {
                _logger.Log(LogType.Info, $"Started to gather data from {link.GetClassName()} with Id: {link.Id}");
                var htmlData = _dataGatherer.Gather(link);
                _logger.Log(LogType.Info, $"Finished to gather data from {link.GetClassName()} with Id: {link.Id}");

                using (var transaction = _htmlDataRepository.GetTransaction())
                {
                    _htmlDataRepository.Insert(htmlData, transaction);
                    _logger.Log(LogType.Info, $"Inserted new {htmlData.GetClassName()} to database");

                    link.Status = Status.Success;
                    _linkRepository.Update(link, transaction);
                    _logger.Log(LogType.Info, $"Updated {link.GetClassName()} with Id: {link.Id}");

                    _commandQueue.Add(new ExtractDataCommand() { Id = Guid.NewGuid().ToString() });
                    _logger.Log(LogType.Info, $"Created new {typeof(ExtractDataCommand).Name} 1x1");
                }
            }
            catch (Exception e)
            {
                using (var transaction = _linkRepository.GetTransaction())
                {
                    link.Status = Status.Failed;
                    _linkRepository.Update(link, transaction);
                    _logger.Log(LogType.Error, $"Failed to gather data from {link.GetClassName()} with Id: {link.Id}|{e.Message}");
                }
                throw;
            }
        }

        private void ChangeUnprocessedLinksStatusToNew(ICollection<Link> links)
        {
            using (var transaction = _linkRepository.GetTransaction())
            {
                foreach (var link in links.Where(x => x.Status == Status.InProgress))
                {
                    link.Status = Status.New;
                    _linkRepository.Update(link, transaction);
                    _logger.Log(LogType.Info, $"Gathering data from {link.GetClassName()} with Id: {link.Id} left unfinished, changed status to new again");
                }
            }
        }
    }
}