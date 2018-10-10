using CityRegionsGenerator.Interfaces;
using CityRegionsGenerator.Modules;
using Ninject;
using System;
using System.Globalization;

namespace CityRegionsGenerator
{
    internal class Program
    {
        internal static void Main(string[] args)
        {
            try
            {
                var minLat = double.Parse(args[0], CultureInfo.InvariantCulture);
                var minLong = double.Parse(args[1], CultureInfo.InvariantCulture);
                var maxLat = double.Parse(args[2], CultureInfo.InvariantCulture);
                var maxLong = double.Parse(args[3], CultureInfo.InvariantCulture);
                var latSize = double.Parse(args[4], CultureInfo.InvariantCulture);
                var longSize = double.Parse(args[5], CultureInfo.InvariantCulture);
                var cityName = args[6];

                var kernel = new StandardKernel(new CityRegionGeneratorModule());
                var generator = kernel.Get<ICityRegionGenerator>();
                generator.GenerateRegions(minLat, minLong, maxLat, maxLong, latSize, longSize, cityName);
            }
            catch (FormatException)
            {
                throw new ArgumentException("Invalid input numbers");
            }
            catch (IndexOutOfRangeException)
            {
                throw new ArgumentException("Some arguments are missing");
            }
        }
    }
}