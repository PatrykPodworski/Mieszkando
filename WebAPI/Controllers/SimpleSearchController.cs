using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MarklogicDataLayer.DataStructs;
using MarklogicDataLayer.Repositories;

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
        public IEnumerable<Offer> Get()
        {
            return new List<Offer>();
        }
    }
}