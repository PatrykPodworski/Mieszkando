using MarklogicDataLayer.Commands.Implementation;
using MarklogicDataLayer.Commands.Interfaces;
using MarklogicDataLayer.DatabaseConnectors;
using MarklogicDataLayer.Repositories;
using Ninject.Modules;

namespace CommandAdder.Modules
{
    public class CommandAdderModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ICommandQueue>().To<CommandQueue>();
            Bind<IDataRepository<ICommand>>().To<DatabaseCommandRepository>();
            Bind<IDatabaseConnectionSettings>().To<DatabaseConnectionSettings>().WithConstructorArgument("key", "mieszkando-db");
        }
    }
}