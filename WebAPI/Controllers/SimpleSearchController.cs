using System.Collections.Generic;
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

        [HttpGet()]
        public IEnumerable<Offer> Get(string maxCost, string noOfRooms)
        {
            var searchModel = new SimpleSearchModel(maxCost, noOfRooms);
            var queryProvider = new SimpleSearchQueryProvider(searchModel);
            var query = queryProvider.GetSearchExpression();

            return _repository.GetWithExpression(query, 1000, 1);
        }
    }
}