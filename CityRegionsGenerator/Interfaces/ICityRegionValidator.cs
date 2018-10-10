using MarklogicDataLayer.DataStructs;
using System.Collections.Generic;

namespace CityRegionsGenerator.Interfaces
{
    public interface ICityRegionValidator
    {
        void Prepare(string cityName, double latSize, double longSize);

        List<CityRegion> GetValidRegions(double minLat, double minLong, double maxLat, double maxLong);
    }
}