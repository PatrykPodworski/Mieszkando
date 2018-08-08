using MarklogicDataLayer.DatabaseConnectors;
using MarklogicDataLayer.DataStructs;
using MarklogicDataLayer.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OfferScraper.Commands.Implementation;
using OfferScraper.Commands.Interfaces;
using OfferScraper.Factories;
using OfferScraper.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests
{
    [TestClass]
    public class DatabaseCommandRepositoryTests
    {
        [TestInitialize]
        public void Initialize()
        {
            DbConnectionSettings = new DatabaseConnectionSettings("vps561493.ovh.net", 8086, "admin", "admin");
            _sut = new DatabaseCommandRepository(DbConnectionSettings);
        }

        [TestCleanup]
        public void Cleanup()
        {
            DbFlush.Perform(DbConnectionSettings);
        }

        protected IDatabaseConnectionSettings DbConnectionSettings;
        private DatabaseCommandRepository _sut;

        [TestMethod]
        public void Insert_uploads_documents_without_throwing()
        {
            try
            {
                var input = new ICommand[] {
                    CommandFactory.Instance.Get(CommandType.ExtractData, 5),
                    CommandFactory.Instance.Get(CommandType.GatherData, 5),
                    CommandFactory.Instance.Get(CommandType.GetLinks, OfferType.Olx),
                };
                _sut.Insert(input);
                Assert.IsTrue(true);
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}
