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
    public class DatabaseAddressRepositoryTests
    {
        [TestInitialize]
        public void Initialize()
        {
            DbConnectionSettings = new DatabaseConnectionSettings("vps561493.ovh.net", 8086, "admin", "admin");
            _sut = new DatabaseAddressRepository(DbConnectionSettings);
        }

        [TestCleanup]
        public void Cleanup()
        {
            DbFlush.Perform(DbConnectionSettings);
        }

        protected IDatabaseConnectionSettings DbConnectionSettings;
        private DatabaseAddressRepository _sut;

        [TestMethod]
        public void Insert_uploads_document_without_throwing()
        {
            try
            {
                var address = new MarklogicDataLayer.DataStructs.Address
                {
                    Id = "1",
                    Country = "Poland",
                    CountryCode = "PL",
                    CountrySecondarySubdivision = "ssubd",
                    CountrySubdivision = "subd",
                    PostalCode = "00-000",
                    Municipality = "mun",
                    MunicipalitySubdivision = "muns",
                    RouteNumbers = new List<string>
                    {
                        "1", "2", "3",
                    },
                    Street = "street",
                };
                _sut.Insert(address);
                Assert.IsTrue(true);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [TestMethod]
        public void GetById_returns_single_address_document()
        {
            var address1 = new MarklogicDataLayer.DataStructs.Address
            {
                Id = "1",
                Country = "Poland",
                CountryCode = "PL",
                CountrySecondarySubdivision = "ssubd",
                CountrySubdivision = "subd",
                PostalCode = "00-000",
                Municipality = "mun",
                MunicipalitySubdivision = "muns",
                RouteNumbers = new List<string>
                    {
                        "1", "2", "3",
                    },
                Street = "street",
            };
            var address2 = new MarklogicDataLayer.DataStructs.Address
            {
                Id = "2",
                Country = "Poland",
                CountryCode = "PL2",
                CountrySecondarySubdivision = "ssubd",
                CountrySubdivision = "subd",
                PostalCode = "00-000",
                Municipality = "mun",
                MunicipalitySubdivision = "muns",
                RouteNumbers = new List<string>
                    {
                        "1", "2", "3",
                    },
                Street = "street",
            };
            _sut.Insert(address1);
            _sut.Insert(address2);
            var result = _sut.GetById(1);
            var expected = address1;

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Delete_removes_address_document()
        {
            var address1 = new MarklogicDataLayer.DataStructs.Address
            {
                Id = "1",
                Country = "Poland",
                CountryCode = "PL",
                CountrySecondarySubdivision = "ssubd",
                CountrySubdivision = "subd",
                PostalCode = "00-000",
                Municipality = "mun",
                MunicipalitySubdivision = "muns",
                RouteNumbers = new List<string>
                    {
                        "1", "2", "3",
                    },
                Street = "street",
            };
            _sut.Insert(address1);
            _sut.Delete(address1);
            var result = _sut.GetAll().ToList();

            Assert.AreEqual(0, result.Count);
        }
    }
}
