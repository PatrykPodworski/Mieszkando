using MarklogicDataLayer.DataStructs;
using OfferScraper.Commands.Interfaces;

namespace OfferScraper.Commands.Implementation
{
    public class CommandBus : ICommandBus
    {
        private readonly IHandlerFactory _handlerFactory;
        private readonly ICommandQueue _commandQueue;

        public CommandBus(IHandlerFactory handlerFactory, ICommandQueue commandQueue)
        {
            _handlerFactory = handlerFactory;
            _commandQueue = commandQueue;
        }

        public void Send(ICommand command)
        {
            try
            {
                _commandQueue.ChangeCommandStatus(command, Status.InProgress);

                var handler = _handlerFactory.Get(command.GetType());
                handler.Handle(command);

                _commandQueue.ChangeCommandStatus(command, Status.Success);
            }
            catch (System.Exception)
            {
                _commandQueue.ChangeCommandStatus(command, Status.Failed);
                throw;
            }
            finally
            {
                if (command.IsInProgress())
                {
                    _commandQueue.ChangeCommandStatus(command, Status.New);
                }
            }
        }
    }
}