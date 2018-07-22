using MarklogicDataLayer.DataStructs;
using System.Collections.Generic;

namespace MarklogicDataLayer.DataProviders
{
    public interface IDataProvider
    {
        ICollection<HTMLData> GetRawDataSamples(OfferType offerType, int numberOfSamples);

        void Commit();

        void Save(Offer offer);

        void MarkAsProcessed(HTMLData sample);
    }
}