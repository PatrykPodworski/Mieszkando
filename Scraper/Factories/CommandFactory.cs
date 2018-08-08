using OfferScraper.Commands.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using MarklogicDataLayer.DataStructs;
using OfferScraper.Commands.Implementation;

namespace OfferScraper.Factories
{
    public class CommandFactory : ICommandFactory<ICommand>
    {
        private static readonly Lazy<CommandFactory> _lazy = new Lazy<CommandFactory>(() => new CommandFactory());
        private CommandFactory() { }

        public static CommandFactory Instance => _lazy.Value;

        public ICommand Get(CommandType type, int? intParam)
        {
            if (type == CommandType.ExtractData)
            {
                if (intParam != null)
                {
                    return new ExtractDataCommand(intParam.GetValueOrDefault());
                }
                else
                {
                    return new ExtractDataCommand();
                }
            }
            else if (type == CommandType.GatherData)
            {
                if (intParam != null)
                {
                    return new GatherDataCommand(intParam.GetValueOrDefault());
                }
                else
                {
                    return new GatherDataCommand();
                }
            }

            throw new ArgumentException("Couldn't resolve dependency for given CommandType");
        }

        public ICommand Get(CommandType type, OfferType? offerType)
        {
            if (type == CommandType.GetLinks)
            {
                if (offerType != null)
                {
                    return new GetLinksCommand(offerType.GetValueOrDefault());
                }
                else
                {
                    return new GetLinksCommand();
                }
            }

            throw new ArgumentException("Couldn't resolve dependency for given CommandType");
        }
    }
}
