using MarklogicDataLayer.DataStructs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OfferScraper.Commands.Implementation;
using OfferScraper.Factories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests
{
    [TestClass]
    public class CommandFactoryTests
    {

        [TestMethod()]
        public void Get_returns_ExtractDataCommand()
        {
            var result = CommandFactory.Instance.Get(CommandType.ExtractData);
            var expected = new ExtractDataCommand();
            Assert.AreEqual(result, expected);
        }

        [TestMethod()]
        public void Get_returns_ExtractDataCommand_with_parameter()
        {
            var result = CommandFactory.Instance.Get(CommandType.ExtractData, 5);
            var expected = new ExtractDataCommand(5);
            Assert.AreEqual(result, expected);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void Get_throws_exception_for_wrong_DataCommand_overload()
        {
            CommandFactory.Instance.Get(CommandType.ExtractData, OfferType.Olx);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void Get_throws_exception_for_wrong_LinkCommand_overload()
        {
            CommandFactory.Instance.Get(CommandType.GetLinks, 5);
        }

        [TestMethod()]
        public void Get_returns_GatherDataCommand()
        {
            var result = CommandFactory.Instance.Get(CommandType.GatherData);
            var expected = new GatherDataCommand();
            Assert.AreEqual(result, expected);
        }

        [TestMethod()]
        public void Get_returns_GatherDataCommand_with_parameter()
        {
            var result = CommandFactory.Instance.Get(CommandType.GatherData, 5);
            var expected = new GatherDataCommand(5);
            Assert.AreEqual(result, expected);
        }

        [TestMethod()]
        public void Get_returns_GetLinksCommand_with_parameter()
        {
            var result = CommandFactory.Instance.Get(CommandType.GetLinks, OfferType.Olx);
            var expected = new GetLinksCommand(OfferType.Olx);
            Assert.AreEqual(result, expected);
        }
    }
}
