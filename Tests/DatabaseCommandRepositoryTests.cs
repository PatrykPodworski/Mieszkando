using MarklogicDataLayer.Commands.Interfaces;
using MarklogicDataLayer.DatabaseConnectors;
using MarklogicDataLayer.DataStructs;
using MarklogicDataLayer.Repositories;
using MarklogicDataLayer.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OfferScraper.Factories;
using System;
using System.Linq;

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

        [TestMethod]
        public void Update_changes_specific_document()
        {
            var command = CommandFactory.Instance.Get(CommandType.ExtractData, 5);
            var id = command.GetId();
            var input = new ICommand[] {
                command,
                CommandFactory.Instance.Get(CommandType.GatherData, 5),
                CommandFactory.Instance.Get(CommandType.GetLinks, OfferType.Olx),
            };
            _sut.Insert(input);
            command.SetStatus(Status.InProgress);
            _sut.Update(command);
            var result = _sut.GetAll();
            Assert.IsTrue(result.Any(x => x.IsInProgress() && x.GetId() == id));
        }

        [TestMethod]
        public void Delete_removes_specified_command_from_database()
        {
            var input = CommandFactory.Instance.Get(CommandType.ExtractData, 5);
            _sut.Insert(input);
            _sut.Delete(input);
            var result = _sut.GetAllFromCollection(input.GetType().ToString().Split(".").Last()).ToList();
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void Delete_removes_only_specified_command_from_database()
        {
            var input = new ICommand[] {
                CommandFactory.Instance.Get(CommandType.ExtractData, 5),
                CommandFactory.Instance.Get(CommandType.GatherData, 5),
                CommandFactory.Instance.Get(CommandType.GetLinks, OfferType.Olx),
            };
            _sut.Insert(input);
            _sut.Delete(input[0]);
            var result = _sut.GetAll().ToList();
            Assert.AreEqual(2, result.Count);
        }
    }
}