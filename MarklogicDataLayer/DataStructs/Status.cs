using System;
using System.Xml.Serialization;

namespace MarklogicDataLayer.DataStructs
{
    [Serializable]
    public enum Status
    {
        [XmlEnum(Name = "New")]
        New,
        [XmlEnum(Name = "InProgress")]
        InProgress,
        [XmlEnum(Name = "Success")]
        Success,
        [XmlEnum(Name = "Failed")]
        Failed
    }
}