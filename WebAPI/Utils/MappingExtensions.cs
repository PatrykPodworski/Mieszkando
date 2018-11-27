using MarklogicDataLayer.DataStructs;
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
                Latitude = offer.Latitude,
                Longitude = offer.Longitude,
                Title = offer.Title,
                TotalCost = offer.TotalCost
            };
        }
    }
}