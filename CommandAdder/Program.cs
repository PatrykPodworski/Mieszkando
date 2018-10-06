using CommandAdder.Modules;
using Ninject;

namespace CommandAdder
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var kernel = new StandardKernel(new CommandAdderModule());
            var adder = kernel.Get<CommandsDatabaseInserter>();

            adder.Add(args);
        }
    }
}