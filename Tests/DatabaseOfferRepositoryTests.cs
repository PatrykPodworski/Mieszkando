using MarklogicDataLayer.Constants;
using MarklogicDataLayer.DatabaseConnectors;
using MarklogicDataLayer.Repositories;
using MarklogicDataLayer.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Tests
{
    [TestCategory("Integration")]
    [TestClass]
    public class DatabaseOfferRepositoryTests
    {
        [TestInitialize]
        public void Initialize()
        {
            DbConnectionSettings = new DatabaseConnectionSettings("vps561493.ovh.net", 8086, "admin", "admin");
            _sut = new DatabaseOfferRepository(DbConnectionSettings);
        }

        [TestCleanup]
        public void Cleanup()
        {
            DbFlush.Perform(DbConnectionSettings);
        }

        protected IDatabaseConnectionSettings DbConnectionSettings;
        private DatabaseOfferRepository _sut;

        [TestMethod]
        public void Insert_uploads_document_without_throwing()
        {
            try
            {
                var offer1 = new MarklogicDataLayer.DataStructs.Offer
                {
                    Id = "1",
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
                    TotalCost = "101.0",
                };
                _sut.Insert(offer1);

                Assert.IsTrue(true);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [TestMethod]
        public void GetAll_returns_all_offer_documents()
        {
            var offer1 = new MarklogicDataLayer.DataStructs.Offer
            {
                Id = "1",
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
                Link = "asd123",
                TotalCost = "101.0",
            };
            var offer2 = new MarklogicDataLayer.DataStructs.Offer
            {
                Id = "2",
                Title = "title2",
                Cost = "101.0",
                BonusCost = "11.0",
                District = "wealthy2",
                Rooms = "422",
                Area = "polite2",
                DateOfPosting = "1971-01-01",
                DateOfScraping = "1972-01-01",
                Latitude = "1",
                Longitude = "1",
                Link = "qwe123",
                TotalCost = "112.0",
            };
            _sut.Insert(offer1);
            _sut.Insert(offer2);
            var result = _sut.GetFromCollection().ToList();
            var expected = new[] {
                offer1,
                offer2
            };

            Assert.AreEqual(2, result.Count);
            CollectionAssert.AreEquivalent(expected, result);
        }

        [TestMethod]
        public void Delete_removes_offer_document()
        {
            var offer1 = new MarklogicDataLayer.DataStructs.Offer
            {
                Id = "1",
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
                TotalCost = "101.0",
            };
            _sut.Insert(offer1);
            _sut.Delete(offer1);
            var result = _sut.GetFromCollection().ToList();

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void GetById_returns_single_offer_document()
        {
            var offer1 = new MarklogicDataLayer.DataStructs.Offer
            {
                Id = "1",
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
                Link = "asd123",
                TotalCost = "101.0",
            };
            var offer2 = new MarklogicDataLayer.DataStructs.Offer
            {
                Id = "2",
                Title = "title2",
                Cost = "101.0",
                BonusCost = "11.0",
                District = "wealthy2",
                Rooms = "422",
                Area = "polite2",
                DateOfPosting = "1971-01-01",
                DateOfScraping = "1972-01-01",
                Latitude = "1",
                Longitude = "1",
                Link = "qwe123",
                TotalCost = "112.0",
            };
            _sut.Insert(offer1);
            _sut.Insert(offer2);
            var result = _sut.GetById(1);
            var expected = offer1;

            Assert.AreEqual(expected, result);
        }

        //[TestMethod]
        public void Get_returns_specified_number_of_documents_queried_by_expression()
        {
            var offer1 = new MarklogicDataLayer.DataStructs.Offer
            {
                Id = "1",
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
                Link = "asd",
                TotalCost = "101.0",
            };
            var offer2 = new MarklogicDataLayer.DataStructs.Offer
            {
                Id = "2",
                Title = "title2",
                Cost = "101.0",
                BonusCost = "11.0",
                District = "wealthy2",
                Rooms = "422",
                Area = "polite2",
                DateOfPosting = "1971-01-01",
                DateOfScraping = "1972-01-01",
                Latitude = "1",
                Longitude = "1",
                Link = "qwe",
            };
            var offer3 = new MarklogicDataLayer.DataStructs.Offer
            {
                Id = "3",
                Title = "title3",
                Cost = "101.0",
                BonusCost = "11.0",
                District = "wealthy2",
                Rooms = "422",
                Area = "polite2",
                DateOfPosting = "1971-01-01",
                DateOfScraping = "1972-01-01",
                Latitude = "1",
                Longitude = "1",
                Link = "123",
            };
            _sut.Insert(new[] { offer1, offer2, offer3 });
            var result = _sut.Get("district", "wealthy2", OfferConstants.CollectionName, 1).ToList();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("wealthy2", result.First().District);
        }
    }
}