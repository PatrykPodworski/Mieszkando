using MarklogicDataLayer.Constants;
using System;
using System.Xml.Serialization;

namespace MarklogicDataLayer.DataStructs
{
    [Serializable, XmlRoot(OfferConstants.OfferRoot)]
    public class Offer
    {
        [XmlElement(OfferConstants.OfferId)]
        public string Id { get; set; }

        [XmlElement(OfferConstants.Title)]
        public string Title { get; set; }

        [XmlElement(OfferConstants.Description)]
        public string Description { get; set; }

        [XmlElement(OfferConstants.District)]
        public string District { get; set; }

        [XmlElement(OfferConstants.Cost)]
        public string Cost { get; set; }

        [XmlElement(OfferConstants.BonusCost)]
        public string BonusCost { get; set; }

        [XmlElement(OfferConstants.Rooms)]
        public string Rooms { get; set; }

        [XmlElement(OfferConstants.Area)]
        public string Area { get; set; }

        [XmlElement(OfferConstants.DateOfPosting)]
        public string DateOfPosting { get; set; }

        [XmlElement(OfferConstants.DateOfScraping)]
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