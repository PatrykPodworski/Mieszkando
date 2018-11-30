using Common.Models;
using RestSharp;
using RouteFinders.Enums;
using RouteFinders.Extensions;
using RouteFinders.Interfaces;
using RouteFinders.Responses;
using System.Globalization;

namespace RouteFinders.Implementations
{
    public class OsrmRouteFinderService : IRouteFinderService
    {
        private IRestClient _restClient;

        public OsrmRouteFinderService(IRestClient restClient)
        {
            _restClient = restClient;
        }

        public Models.Route GetRoute(Coordinates from, Coordinates to, MeanOfTransport meanOfTransport)
        {
            var request = new RestRequest
            {
                Resource = $"route/v1/driving/{from.Longitude.ToString(CultureInfo.InvariantCulture)}" +
                $",{from.Latitude.ToString(CultureInfo.InvariantCulture)};" +
                $"{to.Longitude.ToString(CultureInfo.InvariantCulture)}," +
                $"{to.Latitude.ToString(CultureInfo.InvariantCulture)}?geometries=geojson"
            };

            var response = _restClient.Execute<RouteResponse>(request);

            if (!response.IsSuccessful)
            {
                return null;
            }
            var route = response.Data.MapToRoute();

            return route;
        }

        public bool IsValidFor(MeanOfTransport meanOfTransport)
        {
            return meanOfTransport == MeanOfTransport.Car;
        }
    }
}