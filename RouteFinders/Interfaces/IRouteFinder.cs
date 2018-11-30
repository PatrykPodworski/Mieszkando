using Common.Models;
using RouteFinders.Enums;
using RouteFinders.Models;

namespace RouteFinders.Interfaces
{
    public interface IRouteFinder
    {
        Route GetRoute(Coordinates from, Coordinates to, MeanOfTransport meanOfTransport);
    }
}