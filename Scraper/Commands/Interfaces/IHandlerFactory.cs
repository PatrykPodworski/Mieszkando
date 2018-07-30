using System;

namespace OfferScraper.Commands.Interfaces
{
    public interface IHandlerFactory
    {
        ICommandHandler Get(Type type);
    }
}