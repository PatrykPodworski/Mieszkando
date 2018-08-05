using System;

namespace OfferScraper.Commands.Interfaces
{
    public interface ICommandHandler
    {
        Type GetCommandType();
    }

    public interface ICommandHandler<T> : ICommandHandler
        where T : ICommand
    {
        void Handle(T command);
    }
}