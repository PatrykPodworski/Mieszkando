using HtmlAgilityPack;
using ScrapySharp.Network;
using System;
using System.Text;

namespace OfferScraper.Utilities.Browsers
{
    public class ScrapySharpBrowser : IBrowser
    {
        private ScrapingBrowser _browser;
        private int _maxTries = 5;

        public ScrapySharpBrowser()
        {
            _browser = new ScrapingBrowser { Encoding = Encoding.UTF8 };
        }

        public HtmlNode GetPage(Uri uri)
        {
            for (int i = 0; i < _maxTries; i++)
            {
                var page = TryToGetPage(uri);

                if (page != null)
                {
                    return page;
                }
            }
            throw new Exception($"Service unavailable, tried to get page from url: {uri.ToString()} {_maxTries} times");
        }

        private HtmlNode TryToGetPage(Uri uri)
        {
            try
            {
                return _browser
                    .NavigateToPage(uri)
                    .Html;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}