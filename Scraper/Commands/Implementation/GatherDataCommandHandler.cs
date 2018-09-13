using MarklogicDataLayer.DataStructs;
using OfferScraper.Commands.Interfaces;
using OfferScraper.DataGatherers;
using OfferScraper.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OfferScraper.Commands.Implementation
{
    public class GatherDataCommandHandler : BaseCommand, ICommandHandler<GatherDataCommand>
    {
        private readonly IDataGatherer _dataGatherer;
        private readonly ICommandQueue _commandQueue;
        private readonly IDataRepository<Link> _linkRepository;
        private readonly IDataRepository<HtmlData> _htmlDataRepository;

        public GatherDataCommandHandler(IDataRepository<Link> linkRepository, IDataRepository<HtmlData> htmlDataRepository,
            ICommandQueue commandQueue, IDataGatherer dataGatherer)
        {
            _linkRepository = linkRepository;
            _htmlDataRepository = htmlDataRepository;
            _commandQueue = commandQueue;
            _dataGatherer = dataGatherer;
        }

        public void Handle(ICommand comm)
        {
            CheckCommandType(comm);
            var command = (GatherDataCommand)comm;

            var links = GetLinks(command.NumberOfLinks);
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
                var htmlData = _dataGatherer.Gather(link);

                using (var transaction = _htmlDataRepository.GetTransaction())
                {
                    _htmlDataRepository.Insert(htmlData, transaction);

                    link.Status = Status.Success;
                    _linkRepository.Update(link, transaction);

                    _commandQueue.Add(new ExtractDataCommand() { Id = Guid.NewGuid().ToString() });
                }
            }
            catch (Exception)
            {
                using (var transaction = _linkRepository.GetTransaction())
                {
                    link.Status = Status.Failed;
                    _linkRepository.Update(link, transaction);
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
                }
            }
        }

        public Type GetCommandType()
        {
            return GetType()
                .GetInterfaces()
                .FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(ICommandHandler<>))
                .GetGenericArguments()[0];
        }
    }
}