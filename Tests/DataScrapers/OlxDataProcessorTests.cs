using MarklogicDataLayer.DataStructs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OfferScraper.DataScrapers;
using System.IO;

namespace Tests.DataScrapers
{
    [TestClass]
    public class OlxDataProcessorTests
    {
        private string _expectedTitle;
        private string _expectedCost;
        private string _expectedRooms;
        private HTMLData _data;

        [TestInitialize]
        public void Initialize()
        {
            _data = new HTMLData
            {
                OfferType = OfferType.Olx,
                IsProcessed = false,
                Content = File.ReadAllText("SampleContent.txt")
            };
            _expectedTitle = "Mieszkanie Gdansk Wrzeszcz 1pok , Kilinskiego -super lokalizacja AKTUA";
            _expectedCost = "1300";
            _expectedRooms = "one";
        }

        [TestMethod]
        public void Process_GetCorrectData()
        {
            var processor = new OlxDataProcessor();

            var result = processor.Process(_data);

            Assert.AreEqual(_expectedTitle, result.Title);
            Assert.AreEqual(_expectedCost, result.Cost);
            Assert.AreEqual(_expectedRooms, result.Rooms);
        }
    }
}