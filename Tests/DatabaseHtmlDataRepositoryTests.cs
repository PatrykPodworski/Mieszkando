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
    public class DatabaseHtmlDataRepositoryTests
    {
        [TestInitialize]
        public void Initialize()
        {
            DbConnectionSettings = new DatabaseConnectionSettings("vps561493.ovh.net", 8086, "admin", "admin");
            _sut = new DatabaseHtmlDataRepository(DbConnectionSettings);
        }

        [TestCleanup]
        public void Cleanup()
        {
            DbFlush.Perform(DbConnectionSettings);
        }

        protected IDatabaseConnectionSettings DbConnectionSettings;
        private DatabaseHtmlDataRepository _sut;

        [TestMethod]
        public void Insert_uploads_document_without_throwing()
        {
            try
            {
                var htmlData1 = new MarklogicDataLayer.DataStructs.HtmlData
                {
                    Id = "1",
                    Content = "test1",
                    OfferType = MarklogicDataLayer.DataStructs.OfferType.Olx,
                    LastUpdate = DateTime.Now,
                    Status = MarklogicDataLayer.DataStructs.Status.New
                };
                _sut.Insert(htmlData1);
                Assert.IsTrue(true);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [TestMethod]
        public void GetAll_returns_all_html_data_documents()
        {
            var htmlData1 = new MarklogicDataLayer.DataStructs.HtmlData
            {
                Id = "1",
                Content = "test1",
                OfferType = MarklogicDataLayer.DataStructs.OfferType.Olx,
                LastUpdate = DateTime.Now,
                Status = MarklogicDataLayer.DataStructs.Status.New
            };
            var htmlData2 = new MarklogicDataLayer.DataStructs.HtmlData
            {
                Id = "2",
                Content = "test2",
                OfferType = MarklogicDataLayer.DataStructs.OfferType.OtoDom,
                LastUpdate = DateTime.Now,
                Status = MarklogicDataLayer.DataStructs.Status.New
            };
            _sut.Insert(htmlData1);
            _sut.Insert(htmlData2);
            var result = _sut.GetAll().ToList();
            var expected = new[] {
                htmlData1,
                htmlData2
            };

            Assert.AreEqual(2, result.Count);
            CollectionAssert.AreEquivalent(expected, result);
        }

        [TestMethod]
        public void Delete_removes_html_data_document()
        {
            var htmlData1 = new MarklogicDataLayer.DataStructs.HtmlData
            {
                Id = "1",
                Content = "test1",
                OfferType = MarklogicDataLayer.DataStructs.OfferType.Olx,
                LastUpdate = DateTime.Now,
                Status = MarklogicDataLayer.DataStructs.Status.New
            };
            _sut.Insert(htmlData1);
            _sut.Delete(htmlData1);
            var result = _sut.GetAll().ToList();

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void GetById_returns_single_html_data_document()
        {
            var htmlData1 = new MarklogicDataLayer.DataStructs.HtmlData
            {
                Id = "1",
                Content = "test1",
                OfferType = MarklogicDataLayer.DataStructs.OfferType.Olx,
                LastUpdate = DateTime.Now,
                Status = MarklogicDataLayer.DataStructs.Status.New
            };
            var htmlData2 = new MarklogicDataLayer.DataStructs.HtmlData
            {
                Id = "2",
                Content = "test2",
                OfferType = MarklogicDataLayer.DataStructs.OfferType.OtoDom,
                LastUpdate = DateTime.Now,
                Status = MarklogicDataLayer.DataStructs.Status.New
            };
            _sut.Insert(htmlData1);
            _sut.Insert(htmlData2);
            var result = _sut.GetById(1);
            var expected = htmlData1;

            Assert.AreEqual(expected, result);
        }
    }
}
