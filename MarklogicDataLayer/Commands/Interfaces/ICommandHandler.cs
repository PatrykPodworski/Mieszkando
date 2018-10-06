using System;

namespace MarklogicDataLayer.Commands.Interfaces
{
    public interface ICommandHandler
    {
        Type GetCommandType();

        void Handle(ICommand command);
    }

    public interface ICommandHandler<T> : ICommandHandler
        where T : ICommand
    {
    }
}