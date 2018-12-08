using OfferSearcher.SearchCriteria;
using RouteFinders.Models;

namespace OfferSearcher.Interfaces
{
    public interface IRouteValidator
    {
        bool IsNotAcceptable(Route route, PointOfInterest pointOfInterest);
    }
}