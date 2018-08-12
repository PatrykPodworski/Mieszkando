using System;
using System.Xml.Serialization;
using MarklogicDataLayer.Constants;

namespace MarklogicDataLayer.DataStructs
{
    [Serializable]
    public enum Status
    {
        [XmlEnum(Name = StatusConstants.StatusNew)]
        New,
        [XmlEnum(Name = StatusConstants.StatusInProgress)]
        InProgress,
        [XmlEnum(Name = StatusConstants.StatusSuccess)]
        Success,
        [XmlEnum(Name = StatusConstants.StatusFailed)]
        Failed
    }
}