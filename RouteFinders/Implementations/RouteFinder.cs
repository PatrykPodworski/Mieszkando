using Common.Models;
using RouteFinders.Enums;
using RouteFinders.Interfaces;
using RouteFinders.Models;

namespace RouteFinders.Implementations
{
    public class RouteFinder : IRouteFinder
    {
        private IRouteFinderServiceFactory _serviceFactory;

        public RouteFinder(IRouteFinderServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }

        public Route GetRoute(Coordinates from, Coordinates to, MeanOfTransport meanOfTransport)
        {
            var service = _serviceFactory.GetService(meanOfTransport);

            return service.GetRoute(from, to, meanOfTransport);
        }
    }
}