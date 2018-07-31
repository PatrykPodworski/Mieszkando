using System;
using System.Xml;
using System.Xml.Serialization;

namespace MarklogicDataLayer.DataStructs
{
    [Serializable, XmlRoot("link")]
    public class Link
    {
        [XmlElement("link_id")]
        public string Id { get; set; }

        [XmlElement("link_uri")]
        public string Uri { get; set; }

        [XmlElement("link_kind")]
        public OfferType LinkSourceKind { get; set; }

        [XmlElement("last_update")]
        public DateTime LastUpdate { get; set; }

        [XmlElement("status")]
        public Status Status { get; set; }

        public Link()
        {
        }

        public override string ToString()
        {
            return $"{Id}|{Uri}";
        }

        public override bool Equals(object obj)
        {
            var item = obj as Link;
            if (item == null)
            {
                return false;
            }

            return this.Id == item.Id
                && this.LinkSourceKind == item.LinkSourceKind
                && this.Uri == item.Uri
                && this.LinkStatus == item.LinkStatus
                && this.LastUpdate == item.LastUpdate;
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode()
                ^ this.LinkSourceKind.GetHashCode()
                ^ this.Uri.GetHashCode()
                ^ this.LinkStatus.GetHashCode()
                ^ this.LastUpdate.GetHashCode();
        }
    }
}