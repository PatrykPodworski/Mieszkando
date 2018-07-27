using MarklogicDataLayer.DatabaseConnectors;
using MarklogicDataLayer.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OfferScrapper.Repositories;
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
        public void Test()
        {
            _sut.Insert(new MarklogicDataLayer.DataStructs.Link("test1", "test1", MarklogicDataLayer.DataStructs.LinkKind.Olx));
            var result = _sut.GetAll().ToList();
            var expected = new MarklogicDataLayer.DataStructs.Link("test1", "test1", MarklogicDataLayer.DataStructs.LinkKind.Olx);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(expected, result.First());
        }
    }
}
