﻿using MarklogicDataLayer.DatabaseConnectors;
using MarklogicDataLayer.DataStructs;
using Ninject.Modules;
using OfferScraper.Commands.Implementation;
using OfferScraper.Commands.Interfaces;
using OfferScraper.DataExtractors;
using OfferScraper.DataGatherers;
using OfferScraper.Factories;
using OfferScraper.LinkGatherers;
using OfferScraper.Repositories;

namespace OfferScraper.NinjectModules
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
            Bind<IDataProcessor>().To<OlxDataProcessor>();
            Bind<IDataProcessor>().To<OtoDomDataProcessor>();

            Bind<IDataRepository<ICommand>>().To<DatabaseCommandRepository>();
            Bind<IDataRepository<Link>>().To<DatabaseLinkRepository>();
            Bind<IDataRepository<HtmlData>>().To<DatabaseHtmlDataRepository>();
            Bind<IDataRepository<Offer>>().To<DatabaseOfferRepository>();

            Bind<IDataGatherer>().To<DataGatherer>();

            Bind<IDatabaseConnectionSettings>().To<DatabaseConnectionSettings>().WithConstructorArgument("key", "mieszkando-db");
        }
    }
}