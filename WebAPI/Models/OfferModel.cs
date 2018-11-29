using MarklogicDataLayer.DataStructs;
using System.Collections.Generic;

namespace WebAPI.Models
{
    public class OfferModel
    {
        public string Title { get; set; }
        public double TotalCost { get; set; }
        public string District { get; set; }
        public double Area { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Link { get; set; }
        public int Rooms { get; set; }
        public List<PointOfInterest> PointsOfInterest { get; set; }
    }
}