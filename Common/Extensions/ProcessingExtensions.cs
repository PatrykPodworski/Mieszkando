using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Common.Extensions
{
    public static class ProcessingExtensions
    {
        public static string ToString(this string date, string format)
        {
            DateTime.TryParse(date, new CultureInfo("pl-PL"), DateTimeStyles.None, out DateTime result);
            return result.ToString(format);
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

        public static int ParseToInt(this string text)
        {
            int.TryParse(text, out int parsedValue);

            return parsedValue;
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

        public static string GetClassName(this object obj)
        {
            return obj.GetType().ToString().Split('.').LastOrDefault();
        }
    }
}