using System;
using System.Collections.Generic;
using System.Text;

namespace OfferScraper.Utilities.DataProcessors
{
    public class OlxRoomParser
    {
        public static string Parse(string rooms)
        {
            switch (rooms)
            {
                case "one": return "1";
                case "two": return "2";
                case "three": return "3";
                case "four": return "4";
                case "five": return "5";
                default: return "";
            }
        }
    }
}
