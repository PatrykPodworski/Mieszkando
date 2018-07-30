using OfferScraper.Commands.Interfaces;
using OfferScraper.DataExtractors;

namespace OfferScraper.Commands.Implementation
{
    public class ExtractDataCommandHandler : ICommandHandler<ExtractDataCommand>
    {
        private readonly IDataExtractor _dataExtractor;

        public void Handle(ExtractDataCommand command)
        {
            _dataExtractor.ExtractData(command.NumberOfSamples);
        }
    }
}