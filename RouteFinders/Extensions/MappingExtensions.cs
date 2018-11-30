using Common.Models;
using RouteFinders.Responses;
using System.Linq;

namespace RouteFinders.Extensions
{
    public static class MappingExtensions
    {
        public static Models.Route MapToRoute(this RouteResponse response)
        {
            return new Models.Route
            {
                TravelTime = response.Routes.First().Duration,
                Distance = response.Routes.First().Distance,
                GeoJson = response.Routes.First().Geometry.Coordinates.Select(x => new Coordinates(x[0], x[1])).ToList()
            };
        }
    }
}