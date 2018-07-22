namespace OfferScraper
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var webScrapper = new WebScrapper();
            webScrapper.Run();
        }
    }
}