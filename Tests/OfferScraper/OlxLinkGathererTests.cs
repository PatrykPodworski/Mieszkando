using HtmlAgilityPack;
using MarklogicDataLayer.DataStructs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OfferScraper.LinkGatherers;
using OfferScraper.Utility.Browsers;
using System;
using System.IO;

namespace Tests.OfferScraper
{
    [TestClass]
    public class OlxLinkGathererTests
    {
        private Link _newestLink;
        private int _numberOfLinks;
        private HtmlNode _page;

        [TestInitialize]
        public void Initialize()
        {
            var html = File.ReadAllText("../../../Samples/OlxGatherLinksPage.txt");
            var data = new HtmlDocument();
            data.LoadHtml(html);
            _page = data.DocumentNode;

            _newestLink = new Link();

            _numberOfLinks = 39;
        }

        [TestMethod]
        public void Gather_skips_premium_links_section()
        {
            var mockBrowser = new Mock<IBrowser>();
            mockBrowser.Setup(x => x.GetPage(It.IsAny<Uri>())).Returns(_page);
            var gatherer = new OlxLinkGatherer(mockBrowser.Object);

            var result = gatherer.Gather(_newestLink);

            Assert.AreEqual(_numberOfLinks, result.Count);
        }
    }
}