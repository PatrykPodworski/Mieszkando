using MarklogicDataLayer.DataStructs;
using OfferScraper.Commands.Interfaces;
using System;
using System.Xml.Serialization;

namespace OfferScraper.Commands.Implementation
{
    public abstract class BaseCommand : ICommand
    {
        [XmlElement("creation_date")]
        public DateTime DateOfCreation { get; set; }
        [XmlElement("last_modified")]
        public DateTime LastModified { get; set; }
        [XmlElement("status")]
        public Status Status { get; set; }

        public bool IsNew()
        {
            return Status == Status.New;
        }
    }
}