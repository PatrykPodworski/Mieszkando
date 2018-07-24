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
        private string _expectedArea;
        private HTMLData _data;
        private string _expectedDistrict;

        [TestInitialize]
        public void Initialize()
        {
            _data = new HTMLData
            {
                OfferType = OfferType.Olx,
                IsProcessed = false,
                Content = File.ReadAllText("SampleContent.txt")
            };
            _expectedTitle = "Nowoczesne mieszkanie 3 pokoje dla studentów";
            _expectedCost = "3000";
            _expectedRooms = "three";
            _expectedArea = "67";
            _expectedDistrict = "Przymorze Małe";
        }

        [TestMethod]
        public void Process_GetCorrectData()
        {
            var processor = new OlxDataProcessor();

            var result = processor.Process(_data);

            Assert.AreEqual(_expectedTitle, result.Title);
            Assert.AreEqual(_expectedCost, result.Cost);
            Assert.AreEqual(_expectedRooms, result.Rooms);
            Assert.AreEqual(_expectedArea, result.Area);
            Assert.AreEqual(_expectedDistrict, result.District);
        }
    }
}