using MarklogicDataLayer.Constants;
using System;
using System.Xml;
using System.Xml.Serialization;

namespace MarklogicDataLayer.DataStructs
{
    [Serializable, XmlRoot(LinkConstants.LinkRoot)]
    public class Link
    {
        [XmlElement(LinkConstants.LinkId)]
        public string Id { get; set; }

        [XmlElement(LinkConstants.LinkUri)]
        public string Uri { get; set; }

        [XmlElement(LinkConstants.LinkKind)]
        public OfferType LinkSourceKind { get; set; }

        [XmlElement(LinkConstants.LastUpdate)]
        public DateTime LastUpdate { get; set; }

        [XmlElement(LinkConstants.Status)]
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
            return LinkSourceKind.GetHashCode()
                ^ Uri.GetHashCode()
                ^ Status.GetHashCode()
                ^ LastUpdate.GetHashCode();
        }
    }
}