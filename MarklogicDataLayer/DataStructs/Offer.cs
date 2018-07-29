using System;
using System.Xml.Serialization;

namespace MarklogicDataLayer.DataStructs
{
    [Serializable, XmlRoot("offer")]
    public class Offer
    {
        [XmlElement("id")]
        public string Id { get; set; }

        [XmlElement("title")]
        public string Title { get; set; }

        [XmlElement("description")]
        public string Description { get; set; }

        [XmlElement("district")]
        public string District { get; set; }

        [XmlElement("cost")]
        public string Cost { get; set; }

        [XmlElement("bonusCost")]
        public string BonusCost { get; set; }

        [XmlElement("rooms")]
        public string Rooms { get; set; }

        [XmlElement("area")]
        public string Area { get; set; }

        [XmlElement("dateOfPosting")]
        public string DateOfPosting { get; set; }

        [XmlElement("dateOfScraping")]
        public string DateOfScraping { get; set; }

        public Offer()
        {
        }

        public override bool Equals(object obj)
        {
            var item = obj as Offer;
            if (item == null)
            {
                return false;
            }

            return this.Id == item.Id
                && this.Title == item.Title
                && this.Description == item.Description
                && this.Cost == item.Cost
                && this.BonusCost == item.BonusCost
                && this.Area == item.Area
                && this.Rooms == item.Rooms
                && this.District == item.District
                && this.DateOfPosting == item.DateOfPosting
                && this.DateOfScraping == item.DateOfScraping;
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode()
                ^ this.Title.GetHashCode()
                ^ this.Description.GetHashCode()
                ^ this.Cost.GetHashCode()
                ^ this.BonusCost.GetHashCode()
                ^ this.Area.GetHashCode()
                ^ this.Rooms.GetHashCode()
                ^ this.District.GetHashCode()
                ^ this.DateOfPosting.GetHashCode()
                ^ this.DateOfScraping.GetHashCode();
        }
    }
}