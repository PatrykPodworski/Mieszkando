using MarklogicDataLayer.DataStructs;
using System.Collections.Generic;

namespace OfferSearcher.SearchCriteria
{
    public class AdvancedSearchCriteria
    {
        public int MinCost { get; set; }
        public int MaxCost { get; set; }
        public int NumberOfRooms { get; set; }
        public int MinArea { get; set; }
        public int MaxArea { get; set; }
        public ICollection<PointOfInterest> PointsOfInterest { get; set; }
    }
}