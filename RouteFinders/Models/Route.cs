using Common.Models;
using System.Collections.Generic;

namespace RouteFinders.Models
{
    public class Route
    {
        public double Distance { get; set; }
        public double TravelTime { get; set; }
        public ICollection<Coordinates> GeoJson { get; set; }
    }
}