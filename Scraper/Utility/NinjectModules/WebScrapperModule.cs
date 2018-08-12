using Ninject.Modules;
using OfferScraper.Commands.Implementation;
using OfferScraper.Commands.Interfaces;
using OfferScraper.Repositories;

namespace OfferScraper.Utility.NinjectModules
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

            Bind<IDataRepository<ICommand>>().To<DatabaseCommandRepository>();
        }
    }
}