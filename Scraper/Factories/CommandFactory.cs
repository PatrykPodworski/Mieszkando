using OfferScraper.Commands.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using MarklogicDataLayer.DataStructs;

namespace OfferScraper.Factories
{
    public class CommandFactory : IFactory<ICommand>
    {
        private static readonly Lazy<CommandFactory> _lazy = new Lazy<CommandFactory>(() => new CommandFactory());
        private CommandFactory() { }

        public static CommandFactory Instance => _lazy.Value;

        public ICommand Get(OfferType type)
        {
            throw new NotImplementedException();
        }

        //public ICommand Get(OfferType type)
        //{
        //    switch()
        //}
    }
}
