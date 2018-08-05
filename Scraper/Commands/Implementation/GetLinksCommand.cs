using MarklogicDataLayer.DataStructs;

namespace OfferScraper.Commands.Implementation
{
    public class GetLinksCommand : BaseCommand
    {
        public OfferType Type { get; set; }
    }
}