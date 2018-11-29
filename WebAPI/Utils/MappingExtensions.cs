using MarklogicDataLayer.DataStructs;
using System.Collections.Generic;
using System.Linq;
using WebAPI.Models;

namespace WebAPI.Utils
{
    public static class MappingExtensions
    {
        public static OfferModel MapToOfferModel(this Offer offer, List<PointOfInterest> pointsOfInterest)
        {
            return new OfferModel
            {
                Area = offer.Area,
                District = offer.District,
                Latitude = offer.Latitude,
                Longitude = offer.Longitude,
                Title = offer.Title,
                TotalCost = offer.TotalCost,
                Link = offer.Link,
                Rooms = offer.Rooms,
                PointsOfInterest = pointsOfInterest,
            };
        }

        public static GroupedOffersModel MapToGroupedOffersModel(this IGrouping<string, OfferModel> group)
        {
            return new GroupedOffersModel
            {
                District = group.Key,
                Offers = group.ToList()
            };
        }
    }
}