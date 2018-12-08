using MarklogicDataLayer.DataStructs;
using MarklogicDataLayer.SearchQuery.SearchModels;
using OfferSearcher.Models;
using OfferSearcher.SearchCriteria;
using System.Globalization;

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
    }
}