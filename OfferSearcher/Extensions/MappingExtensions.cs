using Common.Models;
using MarklogicDataLayer.DataStructs;
using MarklogicDataLayer.SearchQuery.SearchModels;
using OfferSearcher.Models;
using OfferSearcher.SearchCriteria;
using System.Globalization;
using TomtomApiWrapper.Responses;

namespace OfferSearcher.Extensions
{
    public static class MappingExtensions
    {
        public static SearchModel MapToSearchModel(this SimpleSearchCriteria criteria)
        {
            return new SearchModel
            {
                MaxCost = criteria.MaxCost.ToString(CultureInfo.InvariantCulture),
                NumberOfRooms = criteria.NumberOfRooms.ToString(CultureInfo.InvariantCulture)
            };
        }

        public static SearchModel MapToSearchModel(this AdvancedSearchCriteria criteria)
        {
            return new SearchModel
            {
                NumberOfRooms = criteria.NumberOfRooms.ToString(CultureInfo.InvariantCulture),
                MinCost = criteria.MinCost.ToString(CultureInfo.InvariantCulture),
                MaxCost = criteria.MaxCost.ToString(CultureInfo.InvariantCulture),
                MinArea = criteria.MinArea.ToString(CultureInfo.InvariantCulture),
                MaxArea = criteria.MaxArea.ToString(CultureInfo.InvariantCulture)
            };
        }

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

        public static Coordinates MapToCoordinates(this CoordinatesResponse response)
        {
            return new Coordinates
            {
                Latitude = response.Lat,
                Longitude = response.Lon
            };
        }
    }
}