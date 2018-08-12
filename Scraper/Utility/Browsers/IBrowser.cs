using HtmlAgilityPack;
using System;

namespace OfferScraper.Utility.Browsers
{
    public interface IBrowser
    {
        HtmlNode GetPage(Uri uri);
    }
}