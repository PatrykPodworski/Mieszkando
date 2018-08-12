using System;
using System.Xml.Serialization;
using MarklogicDataLayer.Constants;

namespace MarklogicDataLayer.DataStructs
{
    [Serializable, XmlRoot(HtmlDataConstants.HtmlData)]
    public class HtmlData
    {
        [XmlElement(HtmlDataConstants.HtmlDataId)]
        public string Id { get; set; }
        [XmlElement(HtmlDataConstants.OfferType)]
        public OfferType OfferType { get; set; }
        [XmlElement(HtmlDataConstants.Status)]
        public Status Status { get; set; }
        [XmlElement(HtmlDataConstants.OfferContent)]
        public string Content { get; set; }
        [XmlElement(HtmlDataConstants.LastUpdate)]
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