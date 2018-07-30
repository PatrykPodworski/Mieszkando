namespace OfferScraper.Commands.Interfaces
{
    public interface ICommandHandler
    {
    }

    public interface ICommandHandler<T> : ICommandHandler
        where T : ICommand
    {
        void Handle(T command);
    }
}