using Ninject;
using OfferScraper.Utilities.NinjectModules;

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