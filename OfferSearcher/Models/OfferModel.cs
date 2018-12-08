using Common.Models;
using RouteFinders.Models;
using System.Collections.Generic;

namespace OfferSearcher.Models
{
    public class OfferModel
    {
        public string Title { get; set; }
        public double TotalCost { get; set; }
        public string District { get; set; }
        public double Area { get; set; }
        public Coordinates Coordinates { get; set; }
        public string Link { get; set; }
        public int Rooms { get; set; }
        public ICollection<Route> Routes { get; set; }

        public OfferModel()
        {
            Routes = new List<Route>();
        }
    }
}