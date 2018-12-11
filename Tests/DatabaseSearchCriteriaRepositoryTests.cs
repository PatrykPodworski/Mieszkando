using MarklogicDataLayer.Constants;
using MarklogicDataLayer.DatabaseConnectors;
using MarklogicDataLayer.Repositories;
using MarklogicDataLayer.SearchQuery.SearchModels;
using MarklogicDataLayer.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Tests
{
    [TestCategory("Integration")]
    [TestClass]
    public class DatabaseSearchCriteriaRepositoryTests
    {
        [TestInitialize]
        public void Initialize()
        {
            DbConnectionSettings = new DatabaseConnectionSettings("vps561493.ovh.net", 8086, "admin", "admin");
            _sut = new DatabaseSearchCriteriaRepository(DbConnectionSettings);
        }

        [TestCleanup]
        public void Cleanup()
        {
            DbFlush.Perform(DbConnectionSettings);
        }

        protected IDatabaseConnectionSettings DbConnectionSettings;
        private DatabaseSearchCriteriaRepository _sut;

        [TestMethod]
        public void Insert_uploads_document_without_throwing()
        {
            try
            {
                var model1 = new SearchModel
                {
                };
                _sut.Insert(model1);

                Assert.IsTrue(true);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [TestMethod]
        public void GetById_returns_proper_document()
        {
            var guid = Guid.NewGuid().ToString();
            var model1 = new SearchModel
            {
                Id = guid,
            };
            _sut.Insert(model1);
            var result = _sut.GetById(guid);

            Assert.AreEqual(model1, result);
        }
    }
}