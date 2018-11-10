using Common.Loggers;
using GarbageCollector.Activities;
using HtmlAgilityPack;
using MarklogicDataLayer.Constants;
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
    [TestClass]
    [TestCategory("Unit")]
    public class GarbageCollectorTests
    {
        private IBrowser _browser;
        private ILogger _logger;
        private IDatabaseConnectionSettings _dbConnectionSettings;
        private DatabaseOfferRepository _dbOfferRepository;
        private DatabaseLinkRepository _dbLinkRepository;
        private OfferLinkCleanupActivity _sut;

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

            _dbConnectionSettings = new DatabaseConnectionSettings("vps561493.ovh.net", 8086, "admin", "admin");
            _dbOfferRepository = new DatabaseOfferRepository(_dbConnectionSettings);
            _dbLinkRepository = new DatabaseLinkRepository(_dbConnectionSettings);
            _sut = new OfferLinkCleanupActivity(_dbOfferRepository, _dbLinkRepository, _browser, _logger);
        }

        [TestCleanup]
        public void Cleanup()
        {
            DbFlush.Perform(_dbConnectionSettings);
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

        [TestCategory("Integration")]
        [TestMethod]
        public void Perform_removes_inactive_Link_document()
        {
            var link = new Link()
            {
                Id = "link_id_1",
                Uri = "http://www.placeholder.com",
                LinkSourceKind = OfferType.Olx,
                LastUpdate = DateTime.Now,
                Status = Status.New
            };
            var offer = new Offer()
            {
                Id = "offer_id_1",
                LinkId = "link_id_1",
                Title = "title",
                Cost = "100.0",
                BonusCost = "1.0",
                District = "wealthy",
                Rooms = "42",
                Area = "polite",
                DateOfPosting = "1970-01-01",
                DateOfScraping = "1970-01-01",
                Latitude = "1",
                Longitude = "1",
            };
            _dbLinkRepository.Insert(link);
            _dbOfferRepository.Insert(offer);
            _sut.Perform();

            var result = _dbLinkRepository.GetAllFromCollection(LinkConstants.CollectionName);

            Assert.AreEqual(0, result.Count());
        }

        [TestCategory("Integration")]
        [TestMethod]
        public void Perform_updates_link_id_in_inactive_Offer()
        {
            var link = new Link()
            {
                Id = "link_id_1",
                Uri = "http://www.placeholder.com",
                LinkSourceKind = OfferType.Olx,
                LastUpdate = DateTime.Now,
                Status = Status.New
            };
            var offer = new Offer()
            {
                Id = "offer_id_1",
                LinkId = "link_id_1",
                Title = "title",
                Cost = "100.0",
                BonusCost = "1.0",
                District = "wealthy",
                Rooms = "42",
                Area = "polite",
                DateOfPosting = "1970-01-01",
                DateOfScraping = "1970-01-01",
                Latitude = "1",
                Longitude = "1",
            };
            _dbLinkRepository.Insert(link);
            _dbOfferRepository.Insert(offer);
            _sut.Perform();

            var result = _dbOfferRepository.Get(OfferConstants.OfferId, "offer_id_1", OfferConstants.CollectionName, 1).First();

            Assert.AreEqual(string.Empty, result.LinkId);
        }
    }
}