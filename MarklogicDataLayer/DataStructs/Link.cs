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

        [XmlElement("date_of_gather")]
        public DateTime DateOfGather { get; set; }

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

            return Id == item.Id
                && LinkSourceKind == item.LinkSourceKind
                && Uri == item.Uri
                && Status == item.Status
                && LastUpdate == item.LastUpdate;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode()
                ^ LinkSourceKind.GetHashCode()
                ^ Uri.GetHashCode()
                ^ Status.GetHashCode()
                ^ LastUpdate.GetHashCode();
        }
    }
}