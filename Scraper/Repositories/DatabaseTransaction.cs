using MarklogicDataLayer;

namespace OfferScraper.Repositories
{
    public class DatabaseTransaction : ITransaction
    {
        private readonly RestConnector _restConnector;
        private readonly MlTransactionScope _transaction;

        public DatabaseTransaction(RestConnector restConnector)
        {
            _restConnector = restConnector;
            _transaction = _restConnector.BeginTransaction();
        }

        public void Dispose()
        {
            _restConnector.CommitTransaction(_transaction);
        }

        public MlTransactionScope GetScope()
        {
            return _transaction;
        }
    }
}