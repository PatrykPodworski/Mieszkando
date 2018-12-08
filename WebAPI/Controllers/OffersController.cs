using Microsoft.AspNetCore.Mvc;
using OfferSearcher.Interfaces;
using OfferSearcher.SearchCriteria;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Utils;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class OffersController : Controller
    {
        private readonly IOfferSearchService _offerSearchService;

        public OffersController(IOfferSearchService offerSearchService)
        {
            _offerSearchService = offerSearchService;
        }

        [HttpGet()]
        public IActionResult Get(SimpleSearchCriteria model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var results = _offerSearchService.SimpleSearch(model);

            var offers = results
                .GroupBy(x => x.District)
                .Select(x => x.MapToGroupedOffersModel())
                .ToList();

            return Ok(offers);
        }

        [HttpPost("advanced")]
        public async Task<IActionResult> GetAdvanced([FromBody] AdvancedSearchCriteria model)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest();
            //}

            //var queryProvider = new AdvancedSearchQueryProvider(model);
            //var query = queryProvider.GetSearchExpression();

            //var databaseResult = _repository.GetWithExpression(query, 1000, 1);

            //foreach (var poi in model.PointsOfInterest)
            //{
            //    var geocodingResult = _tomtomApi.Geocoding("Gdańsk, " + poi.Address);
            //    poi.Coordinates = new Coordinates(geocodingResult.Lat, geocodingResult.Lon);
            //}

            //var offerModels = databaseResult
            //    .Select(x => x.MapToOfferModel())
            //    .ToList();

            //var resultOffers = new List<OfferModel>();
            //foreach (var offer in offerModels)
            //{
            //    foreach (var poi in model.PointsOfInterest)
            //    {
            //        var route = _routeFinder.GetRoute(offer.Coordinates, poi.Coordinates, MeanOfTransport.Car);

            //        if ((route.Distance / 1000.0) > poi.MaxDistanceTo && (route.TravelTime / 60.0) > poi.MaxTravelTime)
            //        {
            //            break;
            //        }

            //        offer.Routes.Add(route);
            //    }

            //    if (offer.Routes.Count != model.PointsOfInterest.Count)
            //    {
            //        continue;
            //    }

            //    resultOffers.Add(offer);
            //}

            //var results = resultOffers.GroupBy(x => x.District)
            //    .Select(x => x.MapToGroupedOffersModel(model.PointsOfInterest))
            //    .ToList();

            return Ok();
        }
    }
}