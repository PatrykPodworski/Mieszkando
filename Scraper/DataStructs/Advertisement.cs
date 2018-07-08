using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace OfferLinkScraper.DataStructs
{
    public class Advertisement
    {
        [XmlElement("title")]
        public string Title { get; set; }
        [XmlElement("description")]
        public string Description { get; set; }
        [XmlElement("district")]
        public string District { get; set; }
        [XmlElement("cost")]
        public string Cost { get; set; }
        [XmlElement("rooms")]
        public string Rooms { get; set; }

        public Advertisement()
        {

        }

        public Advertisement(string title, string description, string district, string cost, string rooms)
        {
            Title = title;
            Description = description;
            District = district;
            Cost = cost;
            Rooms = rooms;
        }
    }
}
