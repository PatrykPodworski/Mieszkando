using CityRegionsGenerator.Interfaces;

namespace CityRegionsGenerator
{
    public class CityRegionGenerator : ICityRegionGenerator
    {
        private ICityRegionValidator _validator;

        public CityRegionGenerator(ICityRegionValidator validator)
        {
            _validator = validator;
        }

        public void GenerateRegions(double minLat, double minLong, double maxLat, double maxLong, double latSize, double longSize, string cityName)
        {
            _validator.Prepare(cityName, latSize, longSize);
            var regions = _validator.GetValidRegions(minLat, minLong, maxLat, maxLong);

            //save to db
        }
    }
}