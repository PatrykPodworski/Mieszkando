using MarklogicDataLayer.DataStructs;
using Ninject;
using OfferScraper.Factories;
using OfferScraper.NinjectModules;
using OfferScraper.Repositories;

namespace OfferScraper
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var kernel = new StandardKernel(new WebScrapperModule());
            var webScrapper = kernel.Get<WebScrapper>();
            webScrapper.Run();
        }
    }
}