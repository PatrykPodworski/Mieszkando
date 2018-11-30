﻿using Common.Models;

namespace MarklogicDataLayer.DataStructs
{
    public class PointOfInterest
    {
        public string Address { get; set; }
        public double MaxDistanceTo { get; set; }
        public double MaxArrivalTime { get; set; }
        public Coordinates Coordinates { get; set; }
    }
}