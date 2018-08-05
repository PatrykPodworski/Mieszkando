namespace OfferScraper.Commands.Implementation
{
    public class GatherDataCommand : BaseCommand
    {
        public int NumberOfLinks { get; set; }

        public GatherDataCommand(int nubmerOfLinks)
        {
            NumberOfLinks = nubmerOfLinks;
        }
    }
}