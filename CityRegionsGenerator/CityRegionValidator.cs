using CityRegionsGenerator.Interfaces;
using Common.Loggers;
using MarklogicDataLayer.DataStructs;
using System;
using System.Collections.Generic;
using System.Globalization;
using TomtomApiWrapper.Interafaces;

namespace CityRegionsGenerator
{
    public class CityRegionValidator : ICityRegionValidator
    {
        private string _cityName;
        private double _latSize;
        private double _longSize;
        private ITomtomApi _tomtomApi;
        private ILogger _logger;

        public CityRegionValidator(ITomtomApi tomtomApi, ILogger logger)
        {
            _tomtomApi = tomtomApi;
            _logger = logger;
            _logger.SetSource(typeof(CityRegionValidator).Name);
        }

        public void Prepare(string cityName, double latSize, double longSize)
        {
            _cityName = cityName;
            _latSize = latSize;
            _longSize = longSize;
        }

        public List<CityRegion> GetValidRegions(double minLat, double minLong, double maxLat, double maxLong)
        {
            var regions = new List<CityRegion>();

            for (double currentLat = minLat; currentLat < maxLat; currentLat += _latSize)
            {
                var latRegions = GetLatRegions(currentLat, minLong, maxLong);

                regions.AddRange(latRegions);

                _logger.Log(LogType.Info, $"There are {regions.Count} regions after checking latitude: {currentLat:F}");
            }

            return regions;
        }

        private List<CityRegion> GetLatRegions(double currentLat, double minLong, double maxLong)
        {
            var regions = new List<CityRegion>();

            for (double currentLong = minLong; currentLong < maxLong; currentLong += _longSize)
            {
                var (centerLat, centerLong) = GetRegionCenterCoords(currentLat, currentLong);

                if (AreValidCityCoords(centerLat, centerLong))
                {
                    _logger.Log(LogType.Info, $"Valid city region found at : {currentLat:F}, {currentLong:F}");

                    regions.Add(new CityRegion
                    {
                        Id = Guid.NewGuid().ToString(),
                        Latitude = currentLat.ToString("F", CultureInfo.InvariantCulture),
                        Longitude = currentLong.ToString("F", CultureInfo.InvariantCulture),
                        LatitudeSize = _latSize.ToString("F", CultureInfo.InvariantCulture),
                        LongitudeSize = _longSize.ToString("F", CultureInfo.InvariantCulture)
                    });
                }
            }

            return regions;
        }

        private (double, double) GetRegionCenterCoords(double currentLat, double currentLong)
        {
            var centerLat = currentLat + _latSize / 2;
            var centerLong = currentLong + _longSize / 2;

            return (centerLat, centerLong);
        }

        private bool AreValidCityCoords(double latitude, double longitude)
        {
            var address = _tomtomApi.ReverseGeocoding(latitude.ToString(CultureInfo.InvariantCulture), longitude.ToString(CultureInfo.InvariantCulture));
            var cityName = address.MunicipalitySubdivision ?? address.Municipality ?? string.Empty;
            return cityName.ToLower() == _cityName.ToLower();
        }
    }
}