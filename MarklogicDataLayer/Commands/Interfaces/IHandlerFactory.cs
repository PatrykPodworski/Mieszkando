using System;

namespace MarklogicDataLayer.Commands.Interfaces
{
    public interface IHandlerFactory
    {
        ICommandHandler Get(Type type);
    }
}