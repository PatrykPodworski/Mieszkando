using MarklogicDataLayer.DataStructs;

namespace OfferScraper.DataExtractors
{
    public interface IDataProcessor
    {
        Offer Process(HtmlData sample);
    }
}