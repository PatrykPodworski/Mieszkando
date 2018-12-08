using Microsoft.AspNetCore.Mvc;
using OfferSearcher.Interfaces;
using OfferSearcher.Models;
using OfferSearcher.SearchCriteria;
using System.Collections.Generic;
using System.Linq;
using WebAPI.Models;
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

            var result = _offerSearchService.SimpleSearch(model);

            var offers = GetGrouppedOffers(result);
            return Ok(offers);
        }

        [HttpPost("advanced")]
        public IActionResult GetAdvanced([FromBody] AdvancedSearchCriteria criteria)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var (result, pointsOfInterest) = _offerSearchService.AdvancedSearch(criteria);

            var offers = GetGrouppedOffers(result, pointsOfInterest);
            return Ok(offers);
        }

        private ICollection<GroupedOffersModel> GetGrouppedOffers(ICollection<OfferModel> offers,
            ICollection<PointOfInterest> pointsOfInterest = null)
        {
            return offers
                .GroupBy(x => x.District)
                .Select(x => x.MapToGroupedOffersModel(pointsOfInterest))
                .ToList();
        }
    }
}