using OfferScraper.Commands.Interfaces;

namespace OfferScraper.Commands.Implementation
{
    public class CommandBus : ICommandBus
    {
        private readonly IHandlerFactory _handlerFactory;

        public CommandBus(IHandlerFactory handlerFactory)
        {
            _handlerFactory = handlerFactory;
        }

        public void Send<T>(T command) where T : ICommand
        {
            var handler = (ICommandHandler<T>)_handlerFactory.Get(typeof(T));
            handler.Handle(command);
        }
    }
}