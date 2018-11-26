using MarklogicDataLayer.Constants;
using MarklogicDataLayer.DataStructs;
using MarklogicDataLayer.Repositories;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult Get()
        {
            var cityRegions = _repository.GetFromCollection(CityRegionConstants.CollectionName, 1);

            return Ok(cityRegions);
        }
    }
}