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
                var link1 = new MarklogicDataLayer.DataStructs.Link
                {
                    Id = "1",
                    Uri = "test1",
                    LinkSourceKind = MarklogicDataLayer.DataStructs.OfferType.Olx,
                    LastUpdate = DateTime.Now,
                    Status = MarklogicDataLayer.DataStructs.Status.New
                };
                _sut.Insert(link1);
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
            var link1 = new MarklogicDataLayer.DataStructs.Link
            {
                Id = "1",
                Uri = "test1",
                LinkSourceKind = MarklogicDataLayer.DataStructs.OfferType.Olx,
                LastUpdate = DateTime.Now,
                Status = MarklogicDataLayer.DataStructs.Status.New
            };
            var link2 = new MarklogicDataLayer.DataStructs.Link
            {
                Id = "2",
                Uri = "test2",
                LinkSourceKind = MarklogicDataLayer.DataStructs.OfferType.OtoDom,
                LastUpdate = DateTime.Now,
                Status = MarklogicDataLayer.DataStructs.Status.New
            };
            _sut.Insert(new[] { link1, link2 });
            var result = _sut.GetAll().ToList();
            var expected = new[] {
                link1,
                link2
            };

            Assert.AreEqual(2, result.Count);
            CollectionAssert.AreEquivalent(expected, result);
        }

        [TestMethod]
        public void GetCount_returns_proper_count()
        {
            var link1 = new MarklogicDataLayer.DataStructs.Link
            {
                Id = "1",
                Uri = "test1",
                LinkSourceKind = MarklogicDataLayer.DataStructs.OfferType.Olx,
                LastUpdate = DateTime.Now,
                Status = MarklogicDataLayer.DataStructs.Status.New
            };
            var link2 = new MarklogicDataLayer.DataStructs.Link
            {
                Id = "2",
                Uri = "test2",
                LinkSourceKind = MarklogicDataLayer.DataStructs.OfferType.OtoDom,
                LastUpdate = DateTime.Now,
                Status = MarklogicDataLayer.DataStructs.Status.New
            };
            _sut.Insert(new[] { link1, link2 });
            var result = _sut.GetCount(LinkConstants.LinksGeneralCollectionName);

            Assert.AreEqual(2, result);
        }

        [TestMethod]
        public void Delete_removes_link_document()
        {
            var link = new MarklogicDataLayer.DataStructs.Link
            {
                Id = "1",
                Uri = "test1",
                LinkSourceKind = MarklogicDataLayer.DataStructs.OfferType.Olx,
                LastUpdate = DateTime.Now,
                Status = MarklogicDataLayer.DataStructs.Status.New
            };
            _sut.Insert(link);
            _sut.Delete(link);
            var result = _sut.GetAll().ToList();

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void GetById_returns_single_link_document()
        {
            var link1 = new MarklogicDataLayer.DataStructs.Link
            {
                Id = "1",
                Uri = "test1",
                LinkSourceKind = MarklogicDataLayer.DataStructs.OfferType.Olx,
                LastUpdate = DateTime.Now,
                Status = MarklogicDataLayer.DataStructs.Status.New
            };
            var link2 = new MarklogicDataLayer.DataStructs.Link
            {
                Id = "2",
                Uri = "test2",
                LinkSourceKind = MarklogicDataLayer.DataStructs.OfferType.OtoDom,
                LastUpdate = DateTime.Now,
                Status = MarklogicDataLayer.DataStructs.Status.New
            };
            _sut.Insert(new[] { link1, link2 });
            var result = _sut.GetById(1);
            var expected = link1;

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Get_returns_specified_number_of_documents_queried_by_expression()
        {
            var link1 = new MarklogicDataLayer.DataStructs.Link
            {
                Id = "1",
                Uri = "test1",
                LinkSourceKind = MarklogicDataLayer.DataStructs.OfferType.Olx,
                LastUpdate = DateTime.Now,
                Status = MarklogicDataLayer.DataStructs.Status.New
            };
            var link2 = new MarklogicDataLayer.DataStructs.Link
            {
                Id = "2",
                Uri = "test2",
                LinkSourceKind = MarklogicDataLayer.DataStructs.OfferType.OtoDom,
                LastUpdate = DateTime.Now,
                Status = MarklogicDataLayer.DataStructs.Status.New
            };
            var link3 = new MarklogicDataLayer.DataStructs.Link
            {
                Id = "3",
                Uri = "test3",
                LinkSourceKind = MarklogicDataLayer.DataStructs.OfferType.OtoDom,
                LastUpdate = DateTime.Now,
                Status = MarklogicDataLayer.DataStructs.Status.InProgress
            };
            _sut.Insert(new[] { link1, link2, link3 });
            var result = _sut.Get("status", "New", LinkConstants.LinksGeneralCollectionName, 1).ToList();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(MarklogicDataLayer.DataStructs.Status.New, result.First().Status);
        }
    }
}