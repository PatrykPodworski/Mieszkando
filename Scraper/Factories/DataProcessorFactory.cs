using MarklogicDataLayer.DataStructs;
using OfferScraper.DataProcessors;
using System;

namespace OfferScraper.Factories
{
    public class DataProcessorFactory : IFactory<IDataProcessor>
    {
        public IDataProcessor Get(OfferType type)
        {
            switch (type)
            {
                case OfferType.Olx:
                    return new OlxDataProcessor();

                case OfferType.OtoDom:
                    return new OtoDomDataProcessor();

                default:
                    throw new ArgumentException("Couldn't resolve dependency for given OfferType");
            }
        }
    }
}