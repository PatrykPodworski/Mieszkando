using System.Collections.Generic;

namespace RouteFinders.Responses
{
    public class Geometry
    {
        public List<List<double>> Coordinates { get; set; }
    }

    public class Route
    {
        public Geometry Geometry { get; set; }
        public double Distance { get; set; }
        public double Duration { get; set; }
    }

    public class RouteResponse
    {
        public List<Route> Routes { get; set; }
    }
}