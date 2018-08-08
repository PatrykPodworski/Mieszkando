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

        public override bool Equals(object obj)
        {
            var item = obj as ExtractDataCommand;
            if (item == null)
            {
                return false;
            }

            return this.LastModified == item.LastModified
                && this.NumberOfSamples == item.NumberOfSamples
                && this.Status == item.Status
                && this.DateOfCreation == item.DateOfCreation;
        }

        public override int GetHashCode()
        {
            return this.LastModified.GetHashCode()
                ^ this.NumberOfSamples.GetHashCode()
                ^ this.Status.GetHashCode()
                ^ this.DateOfCreation.GetHashCode();
        }
    }
}