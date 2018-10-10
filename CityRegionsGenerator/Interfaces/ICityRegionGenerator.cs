namespace CityRegionsGenerator.Interfaces
{
    public interface ICityRegionGenerator
    {
        void GenerateRegions(double minLat, double minLong, double maxLat, double maxLong, double latSize, double longSize, string cityName);
    }
}