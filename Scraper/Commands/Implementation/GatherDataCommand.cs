using OfferScraper.Commands.Interfaces;

namespace OfferScraper.Commands.Implementation
{
    public class GatherDataCommand : ICommand
    {
        public int NumberOfLinks { get; set; }
    }
}