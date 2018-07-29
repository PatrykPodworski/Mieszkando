using System;
using System.Xml.Serialization;

namespace MarklogicDataLayer.DataStructs
{
    [Serializable, XmlRoot("html_data")]
    public class HtmlData
    {
        [XmlElement("html_data_id")]
        public string Id { get; set; }
        [XmlElement("offer_type")]
        public OfferType OfferType { get; set; }
        [XmlElement("status")]
        public Status Status { get; set; }
        [XmlElement("offer_content")]
        public string Content { get; set; }
        [XmlElement("last_update")]
        public DateTime LastUpdate { get; set; }

        public HtmlData()
        {

        }

        public override bool Equals(object obj)
        {
            var item = obj as HtmlData;
            if (item == null)
            {
                return false;
            }

            return this.Id == item.Id
                && this.OfferType == item.OfferType
                && this.Status == item.Status
                && this.Content == item.Content
                && this.LastUpdate == item.LastUpdate;
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode()
                ^ this.OfferType.GetHashCode()
                ^ this.Status.GetHashCode()
                ^ this.Content.GetHashCode()
                ^ this.LastUpdate.GetHashCode();
        }
    }
}