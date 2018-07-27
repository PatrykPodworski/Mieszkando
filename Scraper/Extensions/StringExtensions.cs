using System;

namespace OfferScraper.Extensions
{
    public static class StringExtensions
    {
        public static string ToShortDateString(this string date)
        {
            return DateTime.Parse(date)
                 .ToShortDateString();
        }

        public static string Second(this string[] array)
        {
            return array[1];
        }

        public static string ParseToString(this char[] array)
        {
            return new string(array);
        }
    }
}