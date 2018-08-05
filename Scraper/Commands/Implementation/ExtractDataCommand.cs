namespace OfferScraper.Commands.Implementation
{
    public class ExtractDataCommand : BaseCommand
    {
        public int NumberOfSamples { get; set; }

        public ExtractDataCommand()
        {
            NumberOfSamples = 1;
        }
    }
}