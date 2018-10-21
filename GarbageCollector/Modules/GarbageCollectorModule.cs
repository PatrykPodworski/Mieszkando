using Common.Loggers;
using MarklogicDataLayer.DatabaseConnectors;
using MarklogicDataLayer.DataStructs;
using MarklogicDataLayer.Repositories;
using Ninject.Modules;
using OfferScraper.Utilities.Browsers;

namespace GarbageCollector.Modules
{
    public class GarbageCollectorModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IDataRepository<Offer>>().To<DatabaseOfferRepository>();
            Bind<IDataRepository<Link>>().To<DatabaseLinkRepository>();

            Bind<IBrowser>().To<ScrapySharpBrowser>();
            Bind<ILogger>().To<DefaultOutputLogger>();

            Bind<IDatabaseConnectionSettings>().To<DatabaseConnectionSettings>().WithConstructorArgument("key", "mieszkando-db");
        }
    }
}
