using MarklogicDataLayer.DataStructs;

namespace OfferScraper.Factories
{
    public interface IFactory<T>
    {
        T Get(OfferType type);
    }
}