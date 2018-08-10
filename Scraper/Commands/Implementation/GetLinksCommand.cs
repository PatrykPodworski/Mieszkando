using MarklogicDataLayer.DataStructs;
using System.Xml.Serialization;
using OfferScraper.Constants;

namespace OfferScraper.Commands.Implementation
{
    [XmlRoot(CommandConstants.GetLinksCommand)]
    public class GetLinksCommand : BaseCommand
    {
        [XmlElement(CommandConstants.OfferType)]
        public OfferType Type { get; set; }

        public GetLinksCommand()
        {

        }

        public GetLinksCommand(OfferType offerType)
        {
            Type = offerType;
        }

        public override bool Equals(object obj)
        {
            var item = obj as GetLinksCommand;
            if (item == null)
            {
                return false;
            }

            return this.LastModified == item.LastModified
                && this.Type == item.Type
                && this.Status == item.Status
                && this.DateOfCreation == item.DateOfCreation;
        }

        public override int GetHashCode()
        {
            return this.LastModified.GetHashCode()
                ^ this.Type.GetHashCode()
                ^ this.Status.GetHashCode()
                ^ this.DateOfCreation.GetHashCode();
        }
    }
}