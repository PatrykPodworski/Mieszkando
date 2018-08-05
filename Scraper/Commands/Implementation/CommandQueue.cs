using OfferScraper.Commands.Interfaces;
using OfferScraper.Repositories;
using System.Linq;

namespace OfferScraper.Commands.Implementation
{
    public class CommandQueue : ICommandQueue
    {
        private readonly IDataRepository<ICommand> _repository;

        public CommandQueue(IDataRepository<ICommand> repository)
        {
            _repository = repository;
        }

        public void Add(ICommand command)
        {
            using (var transaction = _repository.GetTransaction())
            {
                _repository.Insert(command, transaction);
            }
        }

        public ICommand GetNext()
        {
            return _repository
                    .GetAll()
                    .FirstOrDefault(x => x.IsNew());
        }

        public bool HasNext()
        {
            return GetNext() == null ? false : true;
        }
    }
}