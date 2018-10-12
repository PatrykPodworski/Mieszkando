using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MarklogicDataLayer.DataStructs
{
    [Serializable, XmlRoot(AddressConstants.AddressRoot)]
    public class Address
    {
        [XmlElement(AddressConstants.Id)]
        public string Id { get; set; }
        [XmlElement(AddressConstants.Country)]
        public string Country { get; set; }
        [XmlElement(AddressConstants.CountryCode)]
        public string CountryCode { get; set; }
        [XmlElement(AddressConstants.CountrySubdivision)]
        public string CountrySubdivision { get; set; }
        [XmlElement(AddressConstants.CountrySecondarySubdivision)]
        public string CountrySecondarySubdivision { get; set; }
        [XmlElement(AddressConstants.Municipality)]
        public string Municipality { get; set; }
        [XmlElement(AddressConstants.MunicipalitySubdivision)]
        public string MunicipalitySubdivision { get; set; }
        [XmlElement(AddressConstants.PostalCode)]
        public string PostalCode { get; set; }
        [XmlElement(AddressConstants.Street)]
        public string Street { get; set; }
        [XmlArray(AddressConstants.RouteNumbers)]
        [XmlArrayItem(AddressConstants.RouteNumber)]
        public List<string> RouteNumbers { get; set; }
    }
}