using MarklogicDataLayer.DataStructs;
using MarklogicDataLayer.Repositories;
using MarklogicDataLayer.SearchQuery.Providers;
using OfferSearcher.Extensions;
using OfferSearcher.Interfaces;
using OfferSearcher.Models;
using OfferSearcher.SearchCriteria;
using RouteFinders.Interfaces;
using RouteFinders.Models;
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
        private readonly IRouteValidator _routeValidator;

        public OfferSearchService(IDataRepository<Offer> repository, ITomtomApi tomtomApi,
            IRouteFinder routeFinder, IRouteValidator routeValidator)
        {
            _repository = repository;
            _tomtomApi = tomtomApi;
            _routeFinder = routeFinder;
            _routeValidator = routeValidator;
        }

        public (ICollection<OfferModel>, ICollection<PointOfInterest>) AdvancedSearch(AdvancedSearchCriteria criteria)
        {
            var searchModel = criteria.MapToSearchModel();

            var queryProvider = new AdvancedSearchQueryProvider(searchModel);
            var query = queryProvider.GetSearchExpression();
            var databaseResult = _repository.GetWithExpression(query, 1000, 1);

            var offerModels = databaseResult
                .Select(x => x.MapToOfferModel())
                .ToList();

            var pointsOfInterest = GeocodePointsOfInterest(criteria.PointsOfInterest);

            var resultOffers = FilterByDistanceAndTime(offerModels, pointsOfInterest);

            return (resultOffers, pointsOfInterest);
        }

        private ICollection<PointOfInterest> GeocodePointsOfInterest(ICollection<PointOfInterest> pointsOfInterest)
        {
            return pointsOfInterest
                .Select(x =>
                    {
                        x.Coordinates = _tomtomApi.Geocoding("Gdańsk, " + x.Address).MapToCoordinates();
                        return x;
                    })
                .ToList();
        }

        private ICollection<OfferModel> FilterByDistanceAndTime(List<OfferModel> offerModels, ICollection<PointOfInterest> pointsOfInterest)
        {
            return offerModels
                .Select(x =>
                    {
                        var routes = GetOfferRoutes(x, pointsOfInterest);
                        x.Routes = routes;
                        return x;
                    })
                .Where(x => !x.Routes.Contains(null))
                .ToList();
        }

        private ICollection<Route> GetOfferRoutes(OfferModel offer, ICollection<PointOfInterest> pointsOfInterest)
        {
            return pointsOfInterest
                .Select(x => GetOfferRoute(offer, x))
                .ToList();
        }

        private Route GetOfferRoute(OfferModel offer, PointOfInterest pointOfInterest)
        {
            var route = _routeFinder.GetRoute(offer.Coordinates, pointOfInterest.Coordinates);

            if (_routeValidator.IsNotAcceptable(route, pointOfInterest))
            {
                return null;
            }

            return route;
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