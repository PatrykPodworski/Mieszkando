using Common.Loggers;
using GarbageCollector.Activities;
using HtmlAgilityPack;
using MarklogicDataLayer.DatabaseConnectors;
using MarklogicDataLayer.DataStructs;
using MarklogicDataLayer.Repositories;
using MarklogicDataLayer.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OfferScraper.Utilities.Browsers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tests
{
    [TestCategory("Integration")]
    [TestClass]
    public class GarbageCollectorTests
    {
        private IBrowser _browser;
        private ILogger _logger;

        [TestInitialize]
        public void Initialize()
        {
            var browserMock = new Mock<IBrowser>();
            browserMock.Setup(x => x.GetPage(It.IsAny<Uri>()))
                .Returns(HtmlNode.CreateNode("To ogłoszenie jest nieaktualne"));
            _browser = browserMock.Object;

            var loggerMock = new Mock<ILogger>();
            loggerMock.Setup(x => x.Log(It.IsAny<LogType>(), It.IsAny<string>()));
            _logger = loggerMock.Object;
        }

        [TestMethod]
        public void Perform_returns_ActivityStatus_Performed_when_at_least_one_action_gets_successfully_performed()
        {
            var linkRepositoryMock = new Mock<IDataRepository<Link>>();
            linkRepositoryMock.Setup(x => x.Get(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<long>()))
                .Returns(new List<Link> { new Link()
                {
                    Uri = "http://www.placeholder.com",
                } }.AsQueryable());
            linkRepositoryMock.Setup(x => x.Delete(It.IsAny<Link>()));

            var offerRepositoryMock = new Mock<IDataRepository<Offer>>();
            offerRepositoryMock.SetupSequence(x => x.GetFromCollection(It.IsAny<string>(), It.IsAny<long>()))
                .Returns(new List<Offer> { new Offer() }.AsQueryable())
                .Returns(new List<Offer>().AsQueryable());
            offerRepositoryMock.Setup(x => x.Update(It.IsAny<Offer>()));

            var sut = new OfferLinkCleanupActivity(offerRepositoryMock.Object, linkRepositoryMock.Object, _browser, _logger);

            var result = sut.Perform();

            Assert.AreEqual(GCActivityStatus.Performed, result);
        }

        [TestMethod]
        public void Perform_returns_ActivityStatus_None_when_no_action_gets_to_be_performed()
        {
            var linkRepositoryMock = new Mock<IDataRepository<Link>>();
            linkRepositoryMock.Setup(x => x.Get(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<long>()))
                .Returns(new List<Link> { new Link()
                {
                    Uri = "http://www.placeholder.com",
                } }.AsQueryable());
            linkRepositoryMock.Setup(x => x.Delete(It.IsAny<Link>()));

            var offerRepositoryMock = new Mock<IDataRepository<Offer>>();
            offerRepositoryMock.SetupSequence(x => x.GetFromCollection(It.IsAny<string>(), It.IsAny<long>()))
                .Returns(new List<Offer>().AsQueryable());
            offerRepositoryMock.Setup(x => x.Update(It.IsAny<Offer>()));

            var sut = new OfferLinkCleanupActivity(offerRepositoryMock.Object, linkRepositoryMock.Object, _browser, _logger);

            var result = sut.Perform();

            Assert.AreEqual(GCActivityStatus.None, result);
        }

        [TestMethod]
        public void Perform_returns_ActivityStatus_Error_when_action_throws_exception()
        {
            var linkRepositoryMock = new Mock<IDataRepository<Link>>();
            linkRepositoryMock.Setup(x => x.Get(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<long>()))
                .Returns(new List<Link> { new Link()
                {
                    Uri = "http://www.placeholder.com",
                } }.AsQueryable());
            linkRepositoryMock.Setup(x => x.Delete(It.IsAny<Link>()));

            var offerRepositoryMock = new Mock<IDataRepository<Offer>>();
            offerRepositoryMock.SetupSequence(x => x.GetFromCollection(It.IsAny<string>(), It.IsAny<long>()))
                .Throws(new Exception());
            offerRepositoryMock.Setup(x => x.Update(It.IsAny<Offer>()));

            var sut = new OfferLinkCleanupActivity(offerRepositoryMock.Object, linkRepositoryMock.Object, _browser, _logger);

            var result = sut.Perform();

            Assert.AreEqual(GCActivityStatus.Error, result);
        }
    }
}
