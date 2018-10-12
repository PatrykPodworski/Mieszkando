using MarklogicDataLayer.Constants;
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

        public Address()
        {

        }

        public override bool Equals(object obj)
        {
            var address = obj as Address;
            return address != null &&
                   Id == address.Id &&
                   Country == address.Country &&
                   CountryCode == address.CountryCode &&
                   CountrySubdivision == address.CountrySubdivision &&
                   CountrySecondarySubdivision == address.CountrySecondarySubdivision &&
                   Municipality == address.Municipality &&
                   MunicipalitySubdivision == address.MunicipalitySubdivision &&
                   PostalCode == address.PostalCode &&
                   Street == address.Street;
        }

        public override int GetHashCode()
        {
            var hashCode = 1841261193;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Id);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Country);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(CountryCode);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(CountrySubdivision);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(CountrySecondarySubdivision);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Municipality);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(MunicipalitySubdivision);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(PostalCode);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Street);
            return hashCode;
        }
    }
}