using MarklogicDataLayer.DatabaseConnectors;
using MarklogicDataLayer.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OfferScraper.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tests
{
    [TestCategory("Integration")]
    [TestClass]
    public class DatabaseLinkRepositoryTests
    {
        [TestInitialize]
        public void Initialize()
        {
            DbConnectionSettings = new DatabaseConnectionSettings("vps561493.ovh.net", 8086, "admin", "admin");
            _sut = new DatabaseLinkRepository(DbConnectionSettings);
        }

        [TestCleanup]
        public void Cleanup()
        {
            DbFlush.Perform(DbConnectionSettings);
        }

        protected IDatabaseConnectionSettings DbConnectionSettings;
        private DatabaseLinkRepository _sut;

        [TestMethod]
        public void Insert_uploads_document_without_throwing()
        {
            try
            {
                _sut.Insert(new MarklogicDataLayer.DataStructs.Link("1", "test1", MarklogicDataLayer.DataStructs.OfferType.Olx));
                Assert.IsTrue(true);
            } 
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [TestMethod]
        public void GetAll_returns_all_link_documents()
        {
            _sut.Insert(new MarklogicDataLayer.DataStructs.Link("1", "test1", MarklogicDataLayer.DataStructs.OfferType.Olx));
            _sut.Insert(new MarklogicDataLayer.DataStructs.Link("2", "test2", MarklogicDataLayer.DataStructs.OfferType.OtoDom));
            var result = _sut.GetAll().ToList();
            var expected = new[] {
                new MarklogicDataLayer.DataStructs.Link("1", "test1", MarklogicDataLayer.DataStructs.OfferType.Olx),
                new MarklogicDataLayer.DataStructs.Link("2", "test2", MarklogicDataLayer.DataStructs.OfferType.OtoDom),
            };

            Assert.AreEqual(2, result.Count);
            CollectionAssert.AreEquivalent(expected, result);
        }

        [TestMethod]
        public void Delete_removes_link_document()
        {
            var link = new MarklogicDataLayer.DataStructs.Link("1", "test1", MarklogicDataLayer.DataStructs.OfferType.Olx);
            _sut.Insert(link);
            _sut.Delete(link);
            var result = _sut.GetAll().ToList();

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void GetById_returns_single_link_document()
        {
            
            _sut.Insert(new MarklogicDataLayer.DataStructs.Link("1", "test1", MarklogicDataLayer.DataStructs.OfferType.Olx));
            _sut.Insert(new MarklogicDataLayer.DataStructs.Link("2", "test2", MarklogicDataLayer.DataStructs.OfferType.OtoDom));
            var result = _sut.GetById(1);
            var expected = new MarklogicDataLayer.DataStructs.Link("1", "test1", MarklogicDataLayer.DataStructs.OfferType.Olx);

            Assert.AreEqual(expected, result);
        }
    }
}
