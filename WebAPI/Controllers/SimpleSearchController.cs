using MarklogicDataLayer.DataStructs;
using MarklogicDataLayer.Repositories;
using MarklogicDataLayer.SearchQuery.Providers;
using MarklogicDataLayer.SearchQuery.SearchModels;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult Get(SimpleSearchModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var queryProvider = new SimpleSearchQueryProvider(model);
            var query = queryProvider.GetSearchExpression();

            var result = _repository.GetWithExpression(query, 1000, 1);

            return Ok(result);
        }
    }
}