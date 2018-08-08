using System.Xml.Serialization;

namespace OfferScraper.Commands.Implementation
{
    [XmlRoot("gather_data_command")]
    public class GatherDataCommand : BaseCommand
    {
        [XmlElement("number_of_links")]
        public int NumberOfLinks { get; set; }

        public GatherDataCommand()
        {

        }

        public GatherDataCommand(int nubmerOfLinks)
        {
            NumberOfLinks = nubmerOfLinks;
        }
    }
}