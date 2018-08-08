using System;
using System.Xml.Serialization;

namespace MarklogicDataLayer.DataStructs
{
    [Serializable]
    public enum OfferType
    {
        [XmlEnum(Name = "Olx")]
        Olx,
        [XmlEnum(Name = "OtoDom")]
        OtoDom,
    }
}