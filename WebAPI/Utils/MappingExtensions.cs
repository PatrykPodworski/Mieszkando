using MarklogicDataLayer.DataStructs;
using System.Globalization;
using WebAPI.Models;

namespace WebAPI.Utils
{
    public static class MappingExtensions
    {
        public static OfferModel MapToOfferModel(this Offer offer)
        {
            double.TryParse(offer.Area, NumberStyles.Any, CultureInfo.InvariantCulture, out var area);
            double.TryParse(offer.Latitude, NumberStyles.Any, CultureInfo.InvariantCulture, out var latitude);
            double.TryParse(offer.Longitude, NumberStyles.Any, CultureInfo.InvariantCulture, out var longitude);
            decimal.TryParse(offer.TotalCost, NumberStyles.Any, CultureInfo.InvariantCulture, out var totalCost);

            return new OfferModel
            {
                Area = area,
                District = offer.District,
                Latitude = latitude,
                Longitude = longitude,
                Title = offer.Title,
                TotalCost = totalCost
            };
        }
    }
}