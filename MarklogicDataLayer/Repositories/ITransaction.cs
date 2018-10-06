using System;

namespace MarklogicDataLayer.Repositories
{
    public interface ITransaction : IDisposable
    {
        MlTransactionScope GetScope();
    }
}