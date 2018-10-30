using MarklogicDataLayer.DataStructs;
using MarklogicDataLayer.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class CityRegionsController : Controller
    {
        private IDataRepository<CityRegion> _repository;

        public CityRegionsController(IDataRepository<CityRegion> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IEnumerable<CityRegion> Get()
        {
            var cityRegions = _repository.GetAll();

            return cityRegions;
        }
    }
}