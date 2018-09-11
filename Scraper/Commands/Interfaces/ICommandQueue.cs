using MarklogicDataLayer.DataStructs;
using OfferScraper.Repositories;

namespace OfferScraper.Commands.Interfaces
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