using MarklogicDataLayer;
using System;

namespace OfferScraper.Repositories
{
    public interface ITransaction : IDisposable
    {
        MlTransactionScope GetScope();
    }
}