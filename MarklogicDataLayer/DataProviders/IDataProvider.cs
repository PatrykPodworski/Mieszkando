using MarklogicDataLayer.DataStructs;
using System.Collections.Generic;

namespace MarklogicDataLayer.DataProviders
{
    public interface IDataProvider
    {
        ICollection<HtmlData> GetRawDataSamples(OfferType offerType, int numberOfSamples);

        void Commit();

        void Save(Offer offer);

        void MarkAsProcessed(HtmlData sample);

        ICollection<Link> GetLinks(int numberOfLinks);

        void MarkAsGathered(Link link);

        void Save(HtmlData data);
    }
}