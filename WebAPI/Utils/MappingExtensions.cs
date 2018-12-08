using MarklogicDataLayer.DataStructs;
using OfferSearcher.Models;
using System.Collections.Generic;
using System.Linq;
using WebAPI.Models;

namespace WebAPI.Utils
{
    public static class MappingExtensions
    {
        public static GroupedOffersModel MapToGroupedOffersModel(this IGrouping<string, OfferModel> group, ICollection<PointOfInterest> pointsOfInterest = null)
        {
            return new GroupedOffersModel
            {
                District = group.Key,
                Offers = group.ToList(),
                PointsOfInterest = pointsOfInterest
            };
        }
    }
}