using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public class GeoCoordinateHelper
    {
        public static double DistanceBetweenCoords(string latitudeFrom, string longitudeFrom, string latitudeTo, string longitudeTo)
        {
            // https://stackoverflow.com/questions/365826/calculate-distance-between-2-gps-coordinates
            var earthRadius = 6371;

            double.TryParse(latitudeFrom, out var latitudeFromDouble);
            double.TryParse(latitudeTo, out var latitudeToDouble);
            double.TryParse(longitudeFrom, out var longitudeFromDouble);
            double.TryParse(longitudeTo, out var longitudeToDouble);

            var latitudeDifference = DegreesToRadians(latitudeToDouble - latitudeFromDouble);
            var longitudeDifference = DegreesToRadians(longitudeToDouble - longitudeFromDouble);

            var latFromDoubleRadians = DegreesToRadians(latitudeFromDouble);
            var latToDoubleRadians = DegreesToRadians(latitudeToDouble);

            var a = Math.Sin(latitudeDifference / 2) * Math.Sin(latitudeDifference / 2) +
                    Math.Sin(longitudeDifference / 2) * Math.Sin(longitudeDifference / 2) * Math.Cos(latFromDoubleRadians) * Math.Cos(latToDoubleRadians);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return earthRadius * c;
        }

        public static string[] GetCoordinatesFromPage(HtmlNode node)
        {
            var siteFragment = node.InnerHtml.Split("APP_INITIALIZATION_STATE")
                .LastOrDefault()
                .Split("\n")
                .FirstOrDefault().Substring(4);
            var coordinates = siteFragment.Remove(siteFragment.Length - 1).Split(',').Skip(1);
            return coordinates.ToArray();
        }

        private static double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180.0;
        }
    }
}
