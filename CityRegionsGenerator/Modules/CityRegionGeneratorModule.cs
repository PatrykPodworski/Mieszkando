using CityRegionsGenerator.Interfaces;
using Common.Loggers;
using MarklogicDataLayer.DatabaseConnectors;
using MarklogicDataLayer.DataStructs;
using MarklogicDataLayer.Repositories;
using Ninject.Modules;
using RestSharp;
using System.Configuration;
using TomtomApiWrapper;
using TomtomApiWrapper.Interafaces;

namespace CityRegionsGenerator.Modules
{
    public class CityRegionGeneratorModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ICityRegionGenerator>().To<CityRegionGenerator>();
            Bind<ICityRegionValidator>().To<CityRegionValidator>();

            Bind<ITomtomApi>().To<TomtomApi>().WithConstructorArgument("apiKey", ConfigurationManager.AppSettings["tomtom-api-key"]);
            Bind<IRestClient>().ToConstructor((ctx) => new RestClient(ConfigurationManager.AppSettings["tomtom-api-base-url"]));

            Bind<IDataRepository<CityRegion>>().To<DatabaseCityRegionRepository>();
            Bind<IDatabaseConnectionSettings>().To<DatabaseConnectionSettings>().WithConstructorArgument("key", "mieszkando-db");

            Bind<ILogger>().To<DefaultOutputLogger>();
        }
    }
}