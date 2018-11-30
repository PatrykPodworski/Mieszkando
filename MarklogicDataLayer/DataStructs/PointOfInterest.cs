namespace MarklogicDataLayer.DataStructs
{
    public class PointOfInterest
    {
        public string Address { get; set; }
        public string MaxDistanceTo { get; set; }
        public string MaxArrivalTime { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}