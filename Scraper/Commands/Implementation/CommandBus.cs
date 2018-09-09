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

        public void Send(ICommand command)
        {
            var handler = _handlerFactory.Get(command.GetType());
            handler.Handle(command);
        }
    }
}