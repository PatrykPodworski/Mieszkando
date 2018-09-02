using MarklogicDataLayer.DataStructs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Utils
{
    public static class LocatorUriBuilder
    {
        private const string _apiKey = "&key=AIzaSyCOSgW_ndo0wm22a1Ncgv260jWgaW2cfnQ";
        private const string _unit = "metric";
        private const string _start = "&origins=";
        private const string _end = "&destinations=";
        private static readonly string _url = $"https://maps.googleapis.com/maps/api/distancematrix/json?units={_unit}";

        public static List<string> BuildUris(IEnumerable<Offer> offers, IEnumerable<PointOfInterest> pois)
        {
            //TODO: think of better exception messages
            if (offers.Count() < 1)
            {
                throw new ArgumentException("Invalid offer amount");
            }
            if (pois.Count() < 1)
            {
                throw new ArgumentException("Specify at least 1 point of interest");
            }

            var result = new List<string>();

            foreach (var offer in offers)
            {
                var poisString = string.Join('|', pois.Select(x => $"{x.Name},{x.Address}"));
                result.Add($"{_url}{_start}{offer.Area}{_end}{poisString}{_apiKey}");
            }

            return result;
        }
    }
}
