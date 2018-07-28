﻿using System.Xml.Serialization;

namespace MarklogicDataLayer.DataStructs
{
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

        public Offer(string title, string description, string district, string cost, string rooms)
        {
            Title = title;
            Description = description;
            District = district;
            Cost = cost;
            Rooms = rooms;
        }
    }
}