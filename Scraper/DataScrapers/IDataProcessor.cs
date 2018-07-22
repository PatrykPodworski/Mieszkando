using MarklogicDataLayer.DataStructs;

namespace OfferScraper.DataScrapers
{
    public interface IDataProcessor
    {
        Offer Process(HTMLData sample);
    }
}