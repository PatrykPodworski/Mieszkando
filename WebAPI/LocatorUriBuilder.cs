using MarklogicDataLayer.DataStructs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI
{
    public static class LocatorUriBuilder
    {
        public static List<string> BuildUris(Offer[] offers, PointOfInterest[] pois)
        {
            var result = new List<string>();

            foreach (var offer in offers)
            {
                result.Add("");
            }

            return result;
        }
    }
}
