using HtmlAgilityPack;
using System;

namespace OfferScraper.Utilities.Browsers
{
    public interface IBrowser
    {
        HtmlNode GetPage(Uri uri);
    }
}