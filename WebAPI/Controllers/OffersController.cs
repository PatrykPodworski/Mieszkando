using MarklogicDataLayer.DataStructs;
using MarklogicDataLayer.Repositories;
using MarklogicDataLayer.SearchQuery.Providers;
using MarklogicDataLayer.SearchQuery.SearchModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RouteFinders.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
                .Select(x => x.MapToOfferModel(model.PointsOfInterest))
                .GroupBy(x => x.District)
                .Select(x => x.MapToGroupedOffersModel())
                .ToList();

            return Ok(offerModels);
        }

        [HttpPost("advanced")]
        public async Task<IActionResult> GetAdvanced()
        {
            var model = new SearchModel();
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                var content = await reader.ReadToEndAsync();
                model = JsonConvert.DeserializeObject<SearchModel>(content);
            }

            var queryProvider = new AdvancedSearchQueryProvider(model);
            var query = queryProvider.GetSearchExpression();

            var databaseResult = _repository.GetWithExpression(query, 1000, 1);
            
            var pointsOfInterestWithCoords = new List<PointOfInterest>();
            foreach (var poi in model.PointsOfInterest)
            {
                var geocodingResult = _tomtomApi.Geocoding(poi.Address);
                pointsOfInterestWithCoords.Add(new PointOfInterest()
                {
                    Address = poi.Address,
                    MaxArrivalTime = poi.MaxArrivalTime,
                    MaxDistanceTo = poi.MaxDistanceTo,
                    Coordinates = new Common.Models.Coordinates(geocodingResult.Lat, geocodingResult.Lon),
                });
            }

            var offerModels = databaseResult
                .Select(x => x.MapToOfferModel(pointsOfInterestWithCoords))
                .ToList();

            var resultOffers = new List<OfferModel>();
            foreach (var offerModel in offerModels)
            {
                var meetsModelRequirements = true;
                foreach (var poi in pointsOfInterestWithCoords)
                {
                    var route = _routeFinder.GetRoute(offerModel.Coordinates, poi.Coordinates, RouteFinders.Enums.MeanOfTransport.Car);

                    if ((route.Distance / 1000.0) > poi.MaxDistanceTo && (route.TravelTime / 60.0) > poi.MaxArrivalTime)
                    {
                        meetsModelRequirements = false;
                        break;
                    }
                }
                if (meetsModelRequirements)
                    resultOffers.Add(offerModel);
            }

            var results = resultOffers.GroupBy(x => x.District)
                .Select(x => x.MapToGroupedOffersModel())
                .ToList();

            return Ok(results);
        }
    }
}