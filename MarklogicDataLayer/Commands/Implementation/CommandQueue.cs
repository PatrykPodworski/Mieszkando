using MarklogicDataLayer.Commands.Interfaces;
using MarklogicDataLayer.Constants;
using MarklogicDataLayer.DataStructs;
using MarklogicDataLayer.Repositories;
using System.Linq;

namespace MarklogicDataLayer.Commands.Implementation
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
            return _repository
                .Get(CommandConstants.Status, StatusConstants.StatusNew, CommandConstants.CollectionName, 1)
                .ToList()
                .FirstOrDefault();
        }

        public bool HasNext()
        {
            return GetNext() == null ? false : true;
        }
    }
}