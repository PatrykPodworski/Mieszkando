using ScrapySharp.Network;
using System.Text;

namespace OfferScraper.Utility
{
    public static class BrowserFactory
    {
        public static ScrapingBrowser GetBrowser()
        {
            return new ScrapingBrowser { Encoding = Encoding.UTF8 };
        }
    }
}