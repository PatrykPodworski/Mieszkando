using RouteFinders.Enums;
using RouteFinders.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RouteFinders.Implementations
{
    public class RouteFinderServiceFactory : IRouteFinderServiceFactory
    {
        private IEnumerable<IRouteFinderService> _services;

        public RouteFinderServiceFactory(IEnumerable<IRouteFinderService> services)
        {
            _services = services;
        }

        public IRouteFinderService GetService(MeanOfTransport meanOfTransport)
        {
            var service = _services.FirstOrDefault(x => x.IsValidFor(meanOfTransport));

            return service ?? throw new Exception($"Mean of transport: {meanOfTransport} is not supported.");
        }
    }
}