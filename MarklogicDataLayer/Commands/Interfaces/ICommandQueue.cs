using MarklogicDataLayer.DataStructs;
using MarklogicDataLayer.Repositories;

namespace MarklogicDataLayer.Commands.Interfaces
{
    public interface ICommandQueue
    {
        ICommand GetNext();

        bool HasNext();

        void Add(ICommand command);

        void Add(ICommand command, ITransaction transaction);

        void ChangeCommandStatus(ICommand command, Status status);
    }
}