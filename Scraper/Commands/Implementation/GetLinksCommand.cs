using MarklogicDataLayer.DataStructs;
using System.Xml.Serialization;

namespace OfferScraper.Commands.Implementation
{
    [XmlRoot("get_links_command")]
    public class GetLinksCommand : BaseCommand
    {
        [XmlElement("offer_type")]
        public OfferType Type { get; set; }

        public GetLinksCommand()
        {

        }

        public GetLinksCommand(OfferType offerType)
        {
            Type = offerType;
        }
    }
}