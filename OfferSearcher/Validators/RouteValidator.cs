using OfferSearcher.Interfaces;
using OfferSearcher.SearchCriteria;
using RouteFinders.Models;

namespace OfferSearcher.Validators
{
    public class RouteValidator : IRouteValidator
    {
        private const int _numberOfSecondsInMinute = 60;
        private const int _numberOfMetersInKilometer = 1000;

        public bool IsNotAcceptable(Route route, PointOfInterest pointOfInterest)
        {
            if (route == null)
            {
                return true;
            }

            if (route.TravelTime > pointOfInterest.MaxTravelTime * _numberOfSecondsInMinute)
            {
                return true;
            }

            if (route.Distance > pointOfInterest.MaxDistanceTo * _numberOfMetersInKilometer)
            {
                return true;
            }

            return false;
        }
    }
}