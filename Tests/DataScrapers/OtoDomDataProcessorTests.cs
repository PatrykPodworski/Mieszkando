using MarklogicDataLayer.DataStructs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OfferScraper.DataExtractors;
using System;
using System.IO;

namespace Tests.DataScrapers
{
    [TestClass]
    public class OtoDomDataProcessorTests
    {
        private HtmlData _data;
        private string _expectedTitle;
        private string _expectedCost;
        private string _expectedRooms;
        private string _expectedArea;
        private string _expectedDistrict;
        private string _expectedId;
        private string _expectedDateOfPosting;
        private object _expectedBonusCost;

        [TestInitialize]
        public void Initialize()
        {
            _data = new HtmlData
            {
                OfferType = OfferType.Olx,
                IsProcessed = false,
                Content = File.ReadAllText("OtoDomSampleContent.txt")
            };

            _expectedId = "54951004";
            _expectedTitle = "BEZ PROWIZJI 3-Pokojowe Mieszkanie Browar Gdański";
            _expectedCost = "2540";
            _expectedBonusCost = "277,53";
            _expectedRooms = "3";
            _expectedArea = "65,29";
            _expectedDistrict = "Wrzeszcz";
            _expectedDateOfPosting = "20.07.2018";
        }

        [TestMethod]
        public void OtoDom_Process_GetCorrectData()
        {
            var processor = new OtoDomDataProcessor();

            var result = processor.Process(_data);

            Assert.AreEqual(_expectedId, result.Id);
            Assert.AreEqual(_expectedTitle, result.Title);
            Assert.AreEqual(_expectedCost, result.Cost);
            Assert.AreEqual(_expectedBonusCost, result.BonusCost);
            Assert.AreEqual(_expectedRooms, result.Rooms);
            Assert.AreEqual(_expectedArea, result.Area);
            Assert.AreEqual(_expectedDistrict, result.District);
            Assert.AreEqual(_expectedDateOfPosting, result.DateOfPosting);
            Assert.AreEqual(DateTime.Now.ToShortDateString(), result.DateOfScraping);
        }
    }
}