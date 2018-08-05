using MarklogicDataLayer.DataStructs;

namespace OfferScraper.DataGatherers
{
    public interface IDataGatherer
    {
        HtmlData Gather(Link link);
    }
}