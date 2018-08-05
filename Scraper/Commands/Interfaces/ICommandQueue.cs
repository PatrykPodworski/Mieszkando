namespace OfferScraper.Commands.Interfaces
{
    public interface ICommandQueue
    {
        ICommand GetNext();

        bool HasNext();

        void Add(ICommand command);
    }
}