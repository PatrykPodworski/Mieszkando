using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MarklogicDataLayer.DataStructs;
using MarklogicDataLayer.Repositories;
using MarklogicDataLayer.SearchQuery.SearchModels;
using MarklogicDataLayer.SearchQuery.Providers;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class SimpleSearchController : Controller
    {
        private readonly IDataRepository<Offer> _repository;

        public SimpleSearchController(IDataRepository<Offer> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IEnumerable<Offer> Get(string totalCost, string costComparisonOperator, string totalArea, string areaComparisonOperator)
        {
            var costSearches = new List<CostSearchModel>
            {
                //new CostSearchModel(totalCost, costComparisonOperator),
            };
            var areaSearches = new List<AreaSearchModel>
            {
                //new AreaSearchModel(totalArea, areaComparisonOperator),
            };
            var searchModel = new OfferSearchModel(costSearches, areaSearches);

            var queryProvider = new OfferSearchQueryProvider(searchModel);
            var query = queryProvider.GetSearchExpression();

            return _repository.GetWithExpression(query, 1000, 1);
        }
    }
}