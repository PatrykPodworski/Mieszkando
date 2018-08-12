using OfferScraper.Commands.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OfferScraper.Commands.Implementation
{
    public class HandlerFactory : IHandlerFactory
    {
        private IEnumerable<ICommandHandler> _handlers;

        public HandlerFactory(IEnumerable<ICommandHandler> handlers)
        {
            _handlers = handlers;
        }

        public ICommandHandler Get(Type type)
        {
            var handler = _handlers.FirstOrDefault(x => x.GetSupportedType() == type);

            return handler ?? throw new ArgumentException("Command type not supported.");
        }
    }
}