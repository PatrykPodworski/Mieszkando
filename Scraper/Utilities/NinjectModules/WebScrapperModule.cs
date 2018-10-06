using MarklogicDataLayer.DatabaseConnectors;
using MarklogicDataLayer.DataStructs;
using Ninject.Modules;
using OfferScraper.Commands.Implementation;
using OfferScraper.Commands.Interfaces;
using OfferScraper.DataGatherers;
using OfferScraper.DataProcessors;
using OfferScraper.Factories;
using OfferScraper.LinkGatherers;
using OfferScraper.Repositories;
using OfferScraper.Utilities.Browsers;
using OfferScraper.Utilities.Loggers;

namespace OfferScraper.Utilities.NinjectModules
{
    public class WebScrapperModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ICommandBus>().To<CommandBus>();
            Bind<IHandlerFactory>().To<HandlerFactory>();
            Bind<ICommandQueue>().To<CommandQueue>();

            Bind<ICommandHandler>().To<GetLinksCommandHandler>();
            Bind<ICommandHandler>().To<GatherDataCommandHandler>();
            Bind<ICommandHandler>().To<ProcessDataCommandHandler>();

            Bind<IFactory<ILinkGatherer>>().To<LinkGathererFactory>();
            Bind<IFactory<IDataProcessor>>().To<DataProcessorFactory>();
            Bind<IDataGatherer>().To<DataGatherer>();

            Bind<IDataProcessor>().To<OlxDataProcessor>();
            Bind<IDataProcessor>().To<OtoDomDataProcessor>();

            Bind<IDataRepository<ICommand>>().To<DatabaseCommandRepository>();
            Bind<IDataRepository<Link>>().To<DatabaseLinkRepository>();
            Bind<IDataRepository<HtmlData>>().To<DatabaseHtmlDataRepository>();
            Bind<IDataRepository<Offer>>().To<DatabaseOfferRepository>();
            Bind<IDataRepository<Utility>>().To<DatabaseUtilityRepository>();

            Bind<IBrowser>().To<ScrapySharpBrowser>();
            Bind<ILogger>().To<DefaultOutputLogger>();

            Bind<IDatabaseConnectionSettings>().To<DatabaseConnectionSettings>().WithConstructorArgument("key", "mieszkando-db");
        }
    }
}