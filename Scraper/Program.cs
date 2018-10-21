using MarklogicDataLayer.DataStructs;
using MarklogicDataLayer.Repositories;
using Ninject;
using OfferScraper.Factories;
using OfferScraper.Utilities.NinjectModules;

namespace OfferScraper
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var kernel = new StandardKernel(new WebScrapperModule());
            var webScrapper = kernel.Get<WebScrapper>();
            var command1 = CommandFactory.Instance.Get(CommandType.GetLinks, OfferType.Olx);
            var command2 = CommandFactory.Instance.Get(CommandType.GetLinks, OfferType.OtoDom);

            var repo = kernel.Get<DatabaseCommandRepository>();
            repo.Insert(command1);
            repo.Insert(command2);
            webScrapper.Run();
        }
    }
}