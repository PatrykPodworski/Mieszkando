using MarklogicDataLayer.DataStructs;
using MarklogicDataLayer.Repositories;
using MarklogicDataLayer.SearchQuery.Providers;
using MarklogicDataLayer.SearchQuery.SearchModels;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using WebAPI.Utils;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class OffersController : Controller
    {
        private readonly IDataRepository<Offer> _repository;

        public OffersController(IDataRepository<Offer> repository)
        {
            _repository = repository;
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

        [HttpGet("advanced")]
        public IActionResult GetAdvanced(SearchModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var queryProvider = new AdvancedSearchQueryProvider(model);
            var query = queryProvider.GetSearchExpression();

            var result = _repository.GetWithExpression(query, 1000, 1);

            foreach (var poi in model.PointsOfInterest)
            {
                // TODO: add usage of tomtom api to add longitude to PoIs
            }

            var offerModels = result
                .Select(x => x.MapToOfferModel(model.PointsOfInterest))
                .ToList();

            return Ok(offerModels);
        }
    }
}