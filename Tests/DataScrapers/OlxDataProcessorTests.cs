using MarklogicDataLayer.DataStructs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OfferScraper.DataProcessors;
using System;
using System.IO;

namespace Tests.DataScrapers
{
    [TestClass]
    [TestCategory("Unit")]
    public class OlxDataProcessorTests
    {
        private HtmlData _data;
        private string _expectedTitle;
        private double _expectedCost;
        private int _expectedRooms;
        private double _expectedArea;
        private string _expectedDistrict;
        private string _expectedId;
        private string _expectedDateOfPosting;
        private double _expectedBonusCost;
        private double _expectedLatitude;
        private double _expectedLongitude;

        [TestInitialize]
        public void Initialize()
        {
            _data = new HtmlData
            {
                OfferType = OfferType.Olx,
                Status = Status.New,
                Content = File.ReadAllText("../../../Samples/OlxSampleHtml.txt"),
                LastUpdate = DateTime.Now,
            };

            _expectedId = "469945769";
            _expectedTitle = "Nowoczesne mieszkanie 3 pokoje dla studentów";
            _expectedCost = 3000;
            _expectedBonusCost = 1200;
            _expectedRooms = 3;
            _expectedArea = 67;
            _expectedDistrict = "Przymorze Małe";
            _expectedDateOfPosting = "23.07.2018";
            _expectedLatitude = 54.40900000;
            _expectedLongitude = 18.58144000;
        }

        [TestMethod]
        public void OLX_Process_GetCorrectId()
        {
            var processor = new OlxDataProcessor();

            var result = processor.Process(_data);

            Assert.AreEqual(_expectedId, result.Id);
        }

        [TestMethod]
        public void OLX_Process_GetCorrectTitle()
        {
            var processor = new OlxDataProcessor();

            var result = processor.Process(_data);

            Assert.AreEqual(_expectedTitle, result.Title);
        }

        [TestMethod]
        public void OLX_Process_GetCorrectCost()
        {
            var processor = new OlxDataProcessor();

            var result = processor.Process(_data);

            Assert.AreEqual(_expectedCost, result.Cost);
        }

        [TestMethod]
        public void OLX_Process_GetCorrectBonusCost()
        {
            var processor = new OlxDataProcessor();

            var result = processor.Process(_data);

            Assert.AreEqual(_expectedBonusCost, result.BonusCost);
        }

        [TestMethod]
        public void OLX_Process_GetCorrectArea()
        {
            var processor = new OlxDataProcessor();

            var result = processor.Process(_data);

            Assert.AreEqual(_expectedArea, result.Area);
        }

        [TestMethod]
        public void OLX_Process_GetCorrectRooms()
        {
            var processor = new OlxDataProcessor();

            var result = processor.Process(_data);

            Assert.AreEqual(_expectedRooms, result.Rooms);
        }

        [TestMethod]
        public void OLX_Process_GetCorrectDistrict()
        {
            var processor = new OlxDataProcessor();

            var result = processor.Process(_data);

            Assert.AreEqual(_expectedDistrict, result.District);
        }

        [TestMethod]
        public void OLX_Process_GetCorrectDateOfPosting()
        {
            var processor = new OlxDataProcessor();

            var result = processor.Process(_data);

            Assert.AreEqual(_expectedDateOfPosting, result.DateOfPosting);
        }

        [TestMethod]
        public void OLX_Process_GetCorrectDateOfScrapping()
        {
            var processor = new OlxDataProcessor();

            var result = processor.Process(_data);

            Assert.AreEqual(DateTime.Now.ToString("dd.MM.yyyy"), result.DateOfScraping);
        }

        [TestMethod]
        public void OLX_Process_GetCorrectLatitude()
        {
            var processor = new OlxDataProcessor();

            var result = processor.Process(_data);

            Assert.AreEqual(_expectedLatitude, result.Latitude);
        }

        [TestMethod]
        public void OLX_Process_GetCorrectLongitude()
        {
            var processor = new OlxDataProcessor();

            var result = processor.Process(_data);

            Assert.AreEqual(_expectedLongitude, result.Longitude);
        }
    }
}