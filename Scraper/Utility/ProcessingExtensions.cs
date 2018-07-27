using System;
using System.Collections.Generic;
using System.Linq;

namespace OfferScraper.Extensions
{
    public static class ProcessingExtensions
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

        public static string ParseToString(this IEnumerable<char> chars)
        {
            return new string(chars.ToArray());
        }

        public static string GetNumber(this string text)
        {
            return text
                .Where(x => char.IsDigit(x) || x == ',')
                .ParseToString();
        }
    }
}