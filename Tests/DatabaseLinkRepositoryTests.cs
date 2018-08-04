﻿using MarklogicDataLayer.DatabaseConnectors;
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
            _sut.Insert(link1);
            _sut.Insert(link2);
            var result = _sut.GetAll().ToList();
            var expected = new[] {
                link1,
                link2
            };

            Assert.AreEqual(2, result.Count);
            CollectionAssert.AreEquivalent(expected, result);
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
            _sut.Insert(link1);
            _sut.Insert(link2);
            var result = _sut.GetById(1);
            var expected = link1;

            Assert.AreEqual(expected, result);
        }
    }
}