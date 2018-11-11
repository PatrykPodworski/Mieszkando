using System;
using System.Xml.Serialization;
using MarklogicDataLayer.Constants;

namespace MarklogicDataLayer.DataStructs
{
    [Serializable]
    public enum OfferType
    {
        [XmlEnum(Name = OfferTypeConstants.Olx)]
        Olx,
        [XmlEnum(Name = OfferTypeConstants.OtoDom)]
        OtoDom,
        [XmlEnum(Name = OfferTypeConstants.Outdated)]
        Outdated,
    }
}