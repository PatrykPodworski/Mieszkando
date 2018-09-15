using System;
using System.Reflection;

namespace OfferScraper.Utilities.Extensions
{
    public class StringValueAttribute : Attribute
    {
        public string StringValue { get; protected set; }

        public StringValueAttribute(string value)
        {
            StringValue = value;
        }
    }

    public static class EnumExtensions
    {
        public static string GetStringValue(this Enum value)
        {
            Type type = value.GetType();

            FieldInfo fieldInfo = type.GetField(value.ToString());

            var attributes = fieldInfo
                .GetCustomAttributes(typeof(StringValueAttribute), false) as StringValueAttribute[];

            return attributes[0]?.StringValue;
        }
    }
}