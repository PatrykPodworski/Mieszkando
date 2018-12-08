using Common.Models;
using MarklogicDataLayer.DataStructs;
using MarklogicDataLayer.Repositories;
using MarklogicDataLayer.SearchQuery.Providers;
using OfferSearcher.Extensions;
using OfferSearcher.Interfaces;
using OfferSearcher.Models;
using OfferSearcher.SearchCriteria;
using RouteFinders.Enums;
using RouteFinders.Interfaces;
using System.Collections.Generic;
using System.Linq;
using TomtomApiWrapper.Interafaces;

namespace OfferSearcher.Services
{
    public class OfferSearchService : IOfferSearchService
    {
        private readonly IDataRepository<Offer> _repository;
        private readonly ITomtomApi _tomtomApi;
        private readonly IRouteFinder _routeFinder;

        public OfferSearchService(IDataRepository<Offer> repository, ITomtomApi tomtomApi,
            IRouteFinder routeFinder)
        {
            _repository = repository;
            _tomtomApi = tomtomApi;
            _routeFinder = routeFinder;
        }

        public ICollection<OfferModel> AdvancedSearch(AdvancedSearchCriteria criteria)
        {
            var searchModel = criteria.MapToSearchModel();

            var queryProvider = new AdvancedSearchQueryProvider(searchModel);
            var query = queryProvider.GetSearchExpression();
            var databaseResult = _repository.GetWithExpression(query, 1000, 1);

            foreach (var poi in criteria.PointsOfInterest)
            {
                var geocodingResult = _tomtomApi.Geocoding("Gdańsk, " + poi.Address);
                poi.Coordinates = new Coordinates(geocodingResult.Lat, geocodingResult.Lon);
            }

            var offerModels = databaseResult
                .Select(x => x.MapToOfferModel())
                .ToList();

            var resultOffers = new List<OfferModel>();

            foreach (var offer in offerModels)
            {
                foreach (var poi in criteria.PointsOfInterest)
                {
                    var route = _routeFinder.GetRoute(offer.Coordinates, poi.Coordinates, MeanOfTransport.Car);

                    if ((route.Distance / 1000.0) > poi.MaxDistanceTo && (route.TravelTime / 60.0) > poi.MaxTravelTime)
                    {
                        break;
                    }

                    offer.Routes.Add(route);
                }

                if (offer.Routes.Count != criteria.PointsOfInterest.Count)
                {
                    continue;
                }

                resultOffers.Add(offer);
            }

            return resultOffers;
        }

        public ICollection<OfferModel> SimpleSearch(SimpleSearchCriteria criteria)
        {
            var searchModel = criteria.MapToSearchModel();

            var queryProvider = new SimpleSearchQueryProvider(searchModel);
            var query = queryProvider.GetSearchExpression();
            var result = _repository.GetWithExpression(query, 1000, 1);

            var offerModels = result
                .Select(x => x.MapToOfferModel())
                .ToList();

            return offerModels;
        }
    }
}