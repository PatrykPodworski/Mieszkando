using Common.Extensions;
using Common.Loggers;
using MarklogicDataLayer.Commands.Interfaces;
using MarklogicDataLayer.DataStructs;

namespace MarklogicDataLayer.Commands.Implementation
{
    public class CommandBus : ICommandBus
    {
        private readonly IHandlerFactory _handlerFactory;
        private readonly ICommandQueue _commandQueue;
        private readonly ILogger _logger;

        public CommandBus(IHandlerFactory handlerFactory, ICommandQueue commandQueue, ILogger logger)
        {
            _handlerFactory = handlerFactory;
            _commandQueue = commandQueue;

            _logger = logger;
            _logger.SetSource(typeof(CommandBus).Name);
        }

        public void Send(ICommand command)
        {
            try
            {
                _logger.Log(LogType.Info, $"Started to handle command {command.GetClassName()} with Id: {command.GetId()}");
                _commandQueue.ChangeCommandStatus(command, Status.InProgress);

                var handler = _handlerFactory.Get(command.GetType());
                handler.Handle(command);

                _commandQueue.ChangeCommandStatus(command, Status.Success);
                _logger.Log(LogType.Info, $"Finished to handle command {command.GetClassName()} with Id: {command.GetId()}");
            }
            catch (System.Exception e)
            {
                _commandQueue.ChangeCommandStatus(command, Status.Failed);
                _logger.Log(LogType.Error, $"Failed to handle command {command.GetClassName()} with Id: {command.GetId()}|{e.Message}");
                throw;
            }
            finally
            {
                if (command.IsInProgress())
                {
                    _commandQueue.ChangeCommandStatus(command, Status.New);
                    _logger.Log(LogType.Info, $"Handling of command {command.GetClassName()} with Id: {command.GetId()} left unfinished, changed status to new again");
                }
            }
        }
    }
}