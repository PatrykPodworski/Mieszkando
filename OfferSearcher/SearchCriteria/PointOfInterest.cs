using Common.Models;

namespace OfferSearcher.SearchCriteria
{
    public class PointOfInterest
    {
        public string Address { get; set; }
        public double MaxDistanceTo { get; set; }
        public double MaxTravelTime { get; set; }
        public Coordinates Coordinates { get; set; }
    }
}