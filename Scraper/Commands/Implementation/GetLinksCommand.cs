using MarklogicDataLayer.DataStructs;
using OfferScraper.Commands.Interfaces;

namespace OfferScraper.Commands.Implementation
{
    public class GetLinksCommand : ICommand
    {
        public OfferType Type { get; set; }
    }
}