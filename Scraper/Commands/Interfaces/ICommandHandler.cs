using System;

namespace OfferScraper.Commands.Interfaces
{
    public interface ICommandHandler
    {
        Type GetSupportedType();
    }

    public interface ICommandHandler<T> : ICommandHandler
        where T : ICommand
    {
        void Handle(T command);
    }
}