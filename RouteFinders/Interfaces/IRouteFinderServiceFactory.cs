using RouteFinders.Enums;

namespace RouteFinders.Interfaces
{
    public interface IRouteFinderServiceFactory
    {
        IRouteFinderService GetService(MeanOfTransport meanOfTransport);
    }
}