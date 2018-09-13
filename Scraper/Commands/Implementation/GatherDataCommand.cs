using OfferScraper.Constants;
using System.Xml.Serialization;

namespace OfferScraper.Commands.Implementation
{
    [XmlRoot(CommandConstants.GatherDataCommand)]
    public class GatherDataCommand : BaseCommand
    {
        [XmlElement(CommandConstants.NumberOfLinks)]
        public int NumberOfLinks { get; set; }

        public GatherDataCommand()
        {
        }

        public GatherDataCommand(int numberOfLinks)
        {
            NumberOfLinks = numberOfLinks;
        }

        public override bool Equals(object obj)
        {
            var item = obj as GatherDataCommand;
            if (item == null)
            {
                return false;
            }

            return this.LastModified == item.LastModified
                && this.NumberOfLinks == item.NumberOfLinks
                && this.Status == item.Status
                && this.DateOfCreation == item.DateOfCreation;
        }

        public override int GetHashCode()
        {
            return this.LastModified.GetHashCode()
                ^ this.NumberOfLinks.GetHashCode()
                ^ this.Status.GetHashCode()
                ^ this.DateOfCreation.GetHashCode();
        }
    }
}