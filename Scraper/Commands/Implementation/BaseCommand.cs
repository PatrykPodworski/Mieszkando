using MarklogicDataLayer.DataStructs;
using OfferScraper.Commands.Interfaces;
using System;
using System.Xml.Serialization;
using OfferScraper.Constants;

namespace OfferScraper.Commands.Implementation
{
    public abstract class BaseCommand : ICommand
    {
        [XmlElement(CommandConstants.CreationDate)]
        public DateTime DateOfCreation { get; set; }
        [XmlElement(CommandConstants.LastModified)]
        public DateTime LastModified { get; set; }
        [XmlElement(CommandConstants.Status)]
        public Status Status { get; set; }

        public bool IsNew()
        {
            return Status == Status.New;
        }
    }
}