using System.Collections.Generic;

namespace TomtomApiWrapper.Models
{
    public class Address
    {
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