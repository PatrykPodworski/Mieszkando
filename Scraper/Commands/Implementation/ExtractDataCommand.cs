using System.Xml.Serialization;

namespace OfferScraper.Commands.Implementation
{
    [XmlRoot("extract_data_command")]
    public class ExtractDataCommand : BaseCommand
    {
        [XmlElement("number_of_samples")]
        public int NumberOfSamples { get; set; }

        public ExtractDataCommand()
        {
            NumberOfSamples = 1;
        }

        public ExtractDataCommand(int numberOfSamples)
        {
            NumberOfSamples = numberOfSamples;
        }
    }
}