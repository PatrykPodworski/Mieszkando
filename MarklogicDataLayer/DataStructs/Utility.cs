using MarklogicDataLayer.Constants;
using System;
using System.Xml.Serialization;

namespace MarklogicDataLayer.DataStructs
{
    [XmlRoot(UtilityConstants.Root)]
    public class Utility
    {
        [XmlElement(UtilityConstants.DateOfLastScraping)]
        public DateTime DateOfLastScraping { get; set; }

        [XmlElement(UtilityConstants.OfferService)]
        public OfferType Type { get; set; }
    }
}
