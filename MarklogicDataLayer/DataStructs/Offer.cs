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

        [XmlElement(OfferConstants.District)]
        public string District { get; set; }

        [XmlElement(OfferConstants.Cost)]
        public string Cost { get; set; }

        [XmlElement(OfferConstants.BonusCost)]
        public string BonusCost { get; set; }

        [XmlElement(OfferConstants.TotalCost)]
        public string TotalCost { get; set; }

        [XmlElement(OfferConstants.Rooms)]
        public string Rooms { get; set; }

        [XmlElement(OfferConstants.Area)]
        public string Area { get; set; }

        [XmlElement(OfferConstants.DateOfPosting)]
        public string DateOfPosting { get; set; }

        [XmlElement(OfferConstants.DateOfScraping)]
        public string DateOfScraping { get; set; }

        [XmlElement(OfferConstants.Latitude)]
        public string Latitude { get; set; }

        [XmlElement(OfferConstants.Longitude)]
        public string Longitude { get; set; }

        [XmlElement(OfferConstants.LinkId)]
        public string LinkId { get; set; }

        [XmlElement(OfferConstants.RegionId)]
        public string RegionId { get; set; }

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
                && this.Cost == item.Cost
                && this.BonusCost == item.BonusCost
                && this.Area == item.Area
                && this.Rooms == item.Rooms
                && this.District == item.District
                && this.DateOfPosting == item.DateOfPosting
                && this.DateOfScraping == item.DateOfScraping
                && this.Longitude == item.Longitude
                && this.Latitude == item.Latitude;
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
                ^ this.DateOfScraping.GetHashCode()
                ^ this.Latitude.GetHashCode()
                ^ this.Longitude.GetHashCode();
        }
    }
}