using OfferScraper.Commands.Interfaces;

namespace OfferScraper.Commands.Implementation
{
    public class ExtractDataCommand : ICommand
    {
        public int NumberOfSamples { get; set; }
    }
}