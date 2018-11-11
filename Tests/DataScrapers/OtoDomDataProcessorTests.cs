﻿using MarklogicDataLayer.DataStructs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OfferScraper.DataProcessors;
using System;
using System.IO;

namespace Tests.DataScrapers
{
    [TestClass]
    [TestCategory("Unit")]
    public class OtodomDataProcessorTests
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
        private string _expectedLongitude;
        private string _expectedLatitude;

        [TestInitialize]
        public void Initialize()
        {
            _data = new HtmlData
            {
                OfferType = OfferType.Olx,
                Status = Status.New,
                Content = File.ReadAllText("../../../Samples/OtodomSampleHtml.txt"),
                LastUpdate = DateTime.Now,
                Link = "asdqwe123",
            };

            _expectedId = "54951004";
            _expectedTitle = "BEZ PROWIZJI 3-Pokojowe Mieszkanie Browar Gdański";
            _expectedCost = "2540";
            _expectedBonusCost = "277,53";
            _expectedRooms = "3";
            _expectedArea = "65,29";
            _expectedDistrict = "Wrzeszcz";
            _expectedDateOfPosting = "20.07.2018";
            _expectedLatitude = "54.37822";
            _expectedLongitude = "18.59636";
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
            Assert.AreEqual(_expectedLatitude, result.Latitude);
            Assert.AreEqual(_expectedLongitude, result.Longitude);
            Assert.AreEqual("asdqwe123", result.Link);
        }

        [TestMethod]
        public void OtoDom_Process_GetCorrectId()
        {
            var processor = new OtoDomDataProcessor();

            var result = processor.Process(_data);

            Assert.AreEqual(_expectedId, result.Id);
        }

        [TestMethod]
        public void OtoDom_Process_GetCorrectTitle()
        {
            var processor = new OtoDomDataProcessor();

            var result = processor.Process(_data);

            Assert.AreEqual(_expectedTitle, result.Title);
        }

        [TestMethod]
        public void OtoDom_Process_GetCorrectCost()
        {
            var processor = new OtoDomDataProcessor();

            var result = processor.Process(_data);

            Assert.AreEqual(_expectedCost, result.Cost);
        }

        [TestMethod]
        public void OtoDom_Process_GetCorrectBonusCost()
        {
            var processor = new OtoDomDataProcessor();

            var result = processor.Process(_data);

            Assert.AreEqual(_expectedBonusCost, result.BonusCost);
        }

        [TestMethod]
        public void OtoDom_Process_GetCorrectRooms()
        {
            var processor = new OtoDomDataProcessor();

            var result = processor.Process(_data);

            Assert.AreEqual(_expectedRooms, result.Rooms);
        }

        [TestMethod]
        public void OtoDom_Process_GetCorrectArea()
        {
            var processor = new OtoDomDataProcessor();

            var result = processor.Process(_data);

            Assert.AreEqual(_expectedArea, result.Area);
        }

        [TestMethod]
        public void OtoDom_Process_GetCorrectDistrict()
        {
            var processor = new OtoDomDataProcessor();

            var result = processor.Process(_data);

            Assert.AreEqual(_expectedDistrict, result.District);
        }

        [TestMethod]
        public void OtoDom_Process_GetCorrectDateOfPosting()
        {
            var processor = new OtoDomDataProcessor();

            var result = processor.Process(_data);

            Assert.AreEqual(_expectedDateOfPosting, result.DateOfPosting);
        }

        [TestMethod]
        public void OtoDom_Process_GetCorrectDateOfScraping()
        {
            var processor = new OtoDomDataProcessor();

            var result = processor.Process(_data);

            Assert.AreEqual(DateTime.Now.ToShortDateString(), result.DateOfScraping);
        }

        [TestMethod]
        public void OtoDom_Process_GetCorrectLatitude()
        {
            var processor = new OtoDomDataProcessor();

            var result = processor.Process(_data);

            Assert.AreEqual(_expectedLatitude, result.Latitude);
        }

        [TestMethod]
        public void OtoDom_Process_GetCorrectLongitude()
        {
            var processor = new OtoDomDataProcessor();

            var result = processor.Process(_data);

            Assert.AreEqual(_expectedLongitude, result.Longitude);
        }
    }
}