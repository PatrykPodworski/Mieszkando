using MarklogicDataLayer.DataStructs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OfferScraper.DataScrapers;
using System.IO;

namespace Tests.DataScrapers
{
    [TestClass]
    public class OlxDataProcessorTests
    {
        private readonly string _expectedTitle = "Mieszkanie Gdansk Wrzeszcz 1pok , Kilinskiego -super lokalizacja AKTUA";
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
        }

        [TestMethod]
        public void Process_GetCorrectTitle()
        {
            var processor = new OlxDataProcessor();

            var result = processor.Process(_data);

            Assert.AreEqual(_expectedTitle, result.Title);
        }
    }
}