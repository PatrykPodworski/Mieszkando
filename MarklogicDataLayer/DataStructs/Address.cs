using MarklogicDataLayer.Constants;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MarklogicDataLayer.DataStructs
{
    public class Address
    {
        public string Id { get; set; }
        public string Country { get; set; }
        public string CountryCode { get; set; }
        public string CountrySubdivision { get; set; }
        public string CountrySecondarySubdivision { get; set; }
        public string Municipality { get; set; }
        public string MunicipalitySubdivision { get; set; }
        public string PostalCode { get; set; }
        public string Street { get; set; }
        public List<string> RouteNumbers { get; set; }
    }
}