using CityRegionsGenerator.Interfaces;
using Common.Loggers;
using MarklogicDataLayer.DataStructs;
using MarklogicDataLayer.Repositories;

namespace CityRegionsGenerator
{
    public class CityRegionGenerator : ICityRegionGenerator
    {
        private ICityRegionValidator _validator;
        private IDataRepository<CityRegion> _repository;
        private ILogger _logger;

        public CityRegionGenerator(ICityRegionValidator validator, IDataRepository<CityRegion> repository, ILogger logger)
        {
            _validator = validator;
            _repository = repository;
            _logger = logger;
            _logger.SetSource(typeof(CityRegionGenerator).Name);
        }

        public void GenerateRegions(double minLat, double minLong, double maxLat, double maxLong, double latSize, double longSize, string cityName)
        {
            _validator.Prepare(cityName, latSize, longSize);

            _logger.Log(LogType.Info, $"Started validating regions");
            var regions = _validator.GetValidRegions(minLat, minLong, maxLat, maxLong);

            _logger.Log(LogType.Info, $"Started inserting regions to repository");
            _repository.Insert(regions);
            _logger.Log(LogType.Info, $"{regions.Count} regions successfully added to repository");
        }
    }
}