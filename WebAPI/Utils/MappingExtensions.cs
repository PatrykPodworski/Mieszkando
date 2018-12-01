using MarklogicDataLayer.DataStructs;
using System.Collections.Generic;
using System.Linq;
using WebAPI.Models;

namespace WebAPI.Utils
{
    public static class MappingExtensions
    {
        public static OfferModel MapToOfferModel(this Offer offer)
        {
            return new OfferModel
            {
                Area = offer.Area,
                District = offer.District,
                Coordinates = new Common.Models.Coordinates(offer.Latitude, offer.Longitude),
                Title = offer.Title,
                TotalCost = offer.TotalCost,
                Link = offer.Link,
                Rooms = offer.Rooms
            };
        }

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