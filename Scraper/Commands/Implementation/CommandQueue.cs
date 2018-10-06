using MarklogicDataLayer.DataStructs;
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

        public void Add(ICommand command, ITransaction transaction)
        {
            _repository.Insert(command, transaction);
        }

        public void ChangeCommandStatus(ICommand command, Status status)
        {
            command.SetStatus(status);
            _repository.Update(command);
        }

        public ICommand GetNext()
        {
            var qwe = _repository
                .GetAll()
                .Where(x => x.IsNew())
                .Count();

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