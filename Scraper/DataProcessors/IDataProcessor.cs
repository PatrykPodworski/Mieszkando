using MarklogicDataLayer.DataStructs;

namespace OfferScraper.DataProcessors
{
    public interface IDataProcessor
    {
        Offer Process(HtmlData sample);
    }
}