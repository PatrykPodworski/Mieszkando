using MarklogicDataLayer.Constants;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MarklogicDataLayer.DataStructs
{
    [Serializable, XmlRoot(CityRegionConstants.Root)]
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

        public override bool Equals(object obj)
        {
            var region = obj as CityRegion;
            return region != null &&
                   Id == region.Id &&
                   Latitude == region.Latitude &&
                   Longitude == region.Longitude &&
                   LatitudeSize == region.LatitudeSize &&
                   LongitudeSize == region.LongitudeSize;
        }

        public override int GetHashCode()
        {
            var hashCode = 786855318;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Id);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Latitude);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Longitude);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(LatitudeSize);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(LongitudeSize);
            return hashCode;
        }
    }
}