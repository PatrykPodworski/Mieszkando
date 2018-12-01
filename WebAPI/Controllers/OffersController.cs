using Common.Models;
using MarklogicDataLayer.DataStructs;
using MarklogicDataLayer.Repositories;
using MarklogicDataLayer.SearchQuery.Providers;
using MarklogicDataLayer.SearchQuery.SearchModels;
using Microsoft.AspNetCore.Mvc;
using RouteFinders.Enums;
using RouteFinders.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TomtomApiWrapper.Interafaces;
using WebAPI.Models;
using WebAPI.Utils;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class OffersController : Controller
    {
        private readonly IDataRepository<Offer> _repository;
        private readonly ITomtomApi _tomtomApi;
        private readonly IRouteFinder _routeFinder;

        public OffersController(IDataRepository<Offer> repository, ITomtomApi tomtomApi, IRouteFinder routeFinder)
        {
            _repository = repository;
            _tomtomApi = tomtomApi;
            _routeFinder = routeFinder;
        }

        [HttpGet()]
        public IActionResult Get(SearchModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var queryProvider = new SimpleSearchQueryProvider(model);
            var query = queryProvider.GetSearchExpression();

            var result = _repository.GetWithExpression(query, 1000, 1);

            var offerModels = result
                .Select(x => x.MapToOfferModel())
                .GroupBy(x => x.District)
                .Select(x => x.MapToGroupedOffersModel(null))
                .ToList();

            return Ok(offerModels);
        }

        [HttpPost("advanced")]
        public async Task<IActionResult> GetAdvanced([FromBody] SearchModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var queryProvider = new AdvancedSearchQueryProvider(model);
            var query = queryProvider.GetSearchExpression();

            var databaseResult = _repository.GetWithExpression(query, 1000, 1);

            foreach (var poi in model.PointsOfInterest)
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
                foreach (var poi in model.PointsOfInterest)
                {
                    var route = _routeFinder.GetRoute(offer.Coordinates, poi.Coordinates, MeanOfTransport.Car);

                    if ((route.Distance / 1000.0) > poi.MaxDistanceTo && (route.TravelTime / 60.0) > poi.MaxTravelTime)
                    {
                        break;
                    }

                    offer.Routes.Add(route);
                }

                if (offer.Routes.Count != model.PointsOfInterest.Count)
                {
                    continue;
                }

                resultOffers.Add(offer);
            }

            var results = resultOffers.GroupBy(x => x.District)
                .Select(x => x.MapToGroupedOffersModel(model.PointsOfInterest))
                .ToList();

            return Ok(results);
        }
    }
}