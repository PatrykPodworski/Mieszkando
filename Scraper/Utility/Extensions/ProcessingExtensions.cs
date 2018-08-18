using System;
using System.Collections.Generic;
using System.Linq;

namespace OfferScraper.Utility.Extensions
{
    public static class ProcessingExtensions
    {
        public static string ToShortDateString(this string date)
        {
            return DateTime.Parse(date)
                 .ToShortDateString();
        }

        public static T Second<T>(this IList<T> array)
        {
            if (array.Count < 1)
            {
                return default(T);
            }

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

        public static string RemoveHtmlTag(this string text, string tagName)
        {
            var startIndex = text.IndexOf($"<{tagName}");
            var endIndex = text.IndexOf($"</{tagName}>", startIndex);

            if (startIndex < 0 || endIndex < 0)
            {
                throw new ArgumentException($"Cannot remove tag <{tagName}, tag not found");
            }

            return text.Remove(startIndex, endIndex - startIndex + $"</{tagName}>".Length);
        }

        public static string TranslateDate(this string text)
        {
            var translations = GetTranslations();

            var wordsToTranslate = translations.Where(x => text.Contains(x.Key));

            foreach (var word in wordsToTranslate)
            {
                text = text.Replace(word.Key, word.Value);
            }

            return text;
        }

        private static IDictionary<string, string> GetTranslations()
        {
            return new Dictionary<string, string>
            {
                {"dzisiaj", DateTime.Today.ToShortDateString() },
                {"wczoraj", DateTime.Today.AddDays(-1).ToShortDateString() }
            };
        }

        public static DateTime ParseToDateTime(this string date)
        {
            DateTime.TryParse(date, out DateTime result);
            return result;
        }
    }
}