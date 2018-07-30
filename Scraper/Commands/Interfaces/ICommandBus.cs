namespace OfferScraper.Commands.Interfaces
{
    public interface ICommandBus
    {
        void Send<T>(T command) where T : ICommand;
    }
}