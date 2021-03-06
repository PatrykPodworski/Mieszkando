﻿using OfferSearcher.Models;
using OfferSearcher.SearchCriteria;
using System.Collections.Generic;

namespace WebAPI.Models
{
    public class GroupedOffersModel
    {
        public string District { get; set; }
        public ICollection<OfferModel> Offers { get; set; }
        public ICollection<PointOfInterest> PointsOfInterest { get; set; }
    }
}