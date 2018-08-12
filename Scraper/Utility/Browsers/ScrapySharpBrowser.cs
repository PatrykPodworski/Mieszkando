using HtmlAgilityPack;
using ScrapySharp.Network;
using System;
using System.Text;

namespace OfferScraper.Utility.Browsers
{
    public class ScrapySharpBrowser : IBrowser
    {
        private ScrapingBrowser _browser;

        public ScrapySharpBrowser()
        {
            _browser = new ScrapingBrowser { Encoding = Encoding.UTF8 };
        }

        public HtmlNode GetPage(Uri uri)
        {
            return _browser
                .NavigateToPage(uri)
                .Html;
        }
    }
}