using Ninject;
using OfferScraper.NinjectModules;

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