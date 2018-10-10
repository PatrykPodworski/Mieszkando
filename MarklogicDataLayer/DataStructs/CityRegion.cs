using MarklogicDataLayer.Constants;
using System.Xml.Serialization;

namespace MarklogicDataLayer.DataStructs
{
    public class CityRegion
    {
        [XmlElement(CityRegionConstants.Id)]
        public string Id { get; set; }

        [XmlElement(CityRegionConstants.Latitude)]
        public string Latitude { get; set; }

        [XmlElement(CityRegionConstants.Longitude)]
        public string Longitude { get; set; }

        [XmlElement(CityRegionConstants.LatitudeSize)]
        public string LatitudeSize { get; set; }

        [XmlElement(CityRegionConstants.LongitudeSize)]
        public string LongitudeSize { get; set; }
    }
}