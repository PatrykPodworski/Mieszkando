using MarklogicDataLayer.DataProviders;
using MarklogicDataLayer.DataStructs;

namespace OfferScraper.DataScrapers
{
    public class BasicDataScraper : IDataScraper
    {
        private IDataProvider _dataProvider;
        private IDataProcessor _dataProcessor;

        public BasicDataScraper(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public void ExtractData(int numberOfSamples)
        {
            // data provider
            var rawDataSamples = _dataProvider.GetRawDataSamples(OfferType.Olx, numberOfSamples);

            // extracting data
            foreach (var sample in rawDataSamples)
            {
                var offer = _dataProcessor.Process(sample);
                _dataProvider.MarkAsProcessed(sample);
                _dataProvider.Save(offer);
            }

            _dataProvider.Commit();
        }
    }
}