using OfferScraper.Commands.Interfaces;
using OfferScraper.DataGatherers;

namespace OfferScraper.Commands.Implementation
{
    public class GatherDataCommandHandler : ICommandHandler<GatherDataCommand>
    {
        private readonly IDataGatherer _dataGatherer;

        public void Handle(GatherDataCommand command)
        {
            _dataGatherer.Gather(command.NumberOfLinks);
        }
    }
}