using Ninject;
using OfferScraper.Utilities.NinjectModules;

namespace CommandAdder
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var kernel = new StandardKernel(new WebScrapperModule());
            var adder = kernel.Get<CommandsDatabaseInserter>();

            adder.Add(args);
        }
    }
}