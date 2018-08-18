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
        private Mock<IBrowser> _mockBrowser;

        [TestInitialize]
        public void Initialize()
        {
            var html = File.ReadAllText("../../../Samples/OlxGatherLinksPage.txt");
            var document = new HtmlDocument();
            document.LoadHtml(html);
            _mockBrowser = new Mock<IBrowser>();
            _mockBrowser.Setup(x => x.GetPage(It.IsAny<Uri>())).Returns(document.DocumentNode);

            _newestLink = new Link();

            _numberOfLinks = 39;
        }

        [TestMethod]
        public void Gather_skips_premium_links_section()
        {
            var gatherer = new OlxLinkGatherer(_mockBrowser.Object);

            var result = gatherer.Gather(_newestLink);

            Assert.AreEqual(_numberOfLinks, result.Count);
        }

        [TestMethod]
        public void Gather_get_only_newer_links()
        {
            var gatherer = new OlxLinkGatherer(_mockBrowser.Object);
            var numberOfNewerLinks = 4;
            var link = new Link { DateOfGather = DateTime.Today.AddHours(13) };

            var result = gatherer.Gather(link);

            Assert.AreEqual(numberOfNewerLinks, result.Count);
        }
    }
}