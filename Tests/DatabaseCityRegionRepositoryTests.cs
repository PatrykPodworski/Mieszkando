using MarklogicDataLayer.Constants;
using MarklogicDataLayer.DatabaseConnectors;
using MarklogicDataLayer.Repositories;
using MarklogicDataLayer.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tests
{
    [TestCategory("Integration")]
    [TestClass]
    public class DatabaseCityRegionRepositoryTests
    {
        [TestInitialize]
        public void Initialize()
        {
            DbConnectionSettings = new DatabaseConnectionSettings("vps561493.ovh.net", 8086, "admin", "admin");
            _sut = new DatabaseCityRegionRepository(DbConnectionSettings);
        }

        [TestCleanup]
        public void Cleanup()
        {
            DbFlush.Perform(DbConnectionSettings);
        }

        protected IDatabaseConnectionSettings DbConnectionSettings;
        private DatabaseCityRegionRepository _sut;

        [TestMethod]
        public void Insert_uploads_document_without_throwing()
        {
            try
            {
                var region1 = new MarklogicDataLayer.DataStructs.CityRegion
                {
                    Id = "1",
                    Latitude = "50",
                    LatitudeSize = "50",
                    Longitude = "40",
                    LongitudeSize = "40",
                };
                _sut.Insert(region1);
                Assert.IsTrue(true);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [TestMethod]
        public void Delete_removes_region_document()
        {
            var region1 = new MarklogicDataLayer.DataStructs.CityRegion
            {
                Id = "1",
                Latitude = "50",
                LatitudeSize = "50",
                Longitude = "40",
                LongitudeSize = "40",
            };
            _sut.Insert(region1);
            _sut.Delete(region1);
            var result = _sut.GetFromCollection().ToList();

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void GetById_returns_single_region_document()
        {
            var region1 = new MarklogicDataLayer.DataStructs.CityRegion
            {
                Id = "1",
                Latitude = "50",
                LatitudeSize = "50",
                Longitude = "40",
                LongitudeSize = "40",
            };
            var region2 = new MarklogicDataLayer.DataStructs.CityRegion
            {
                Id = "2",
                Longitude = "50",
                LongitudeSize = "50",
                Latitude = "40",
                LatitudeSize = "40",
            };
            _sut.Insert(region1);
            _sut.Insert(region2);
            var result = _sut.GetById(1);
            var expected = region1;

            Assert.AreEqual(expected, result);
        }
    }
}
