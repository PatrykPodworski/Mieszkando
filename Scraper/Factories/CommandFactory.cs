using MarklogicDataLayer.Commands.Implementation;
using MarklogicDataLayer.Commands.Interfaces;
using MarklogicDataLayer.DataStructs;
using System;

namespace OfferScraper.Factories
{
    public class CommandFactory : ICommandFactory<ICommand>
    {
        private static readonly Lazy<CommandFactory> _lazy = new Lazy<CommandFactory>(() => new CommandFactory());

        private CommandFactory()
        {
        }

        public static CommandFactory Instance => _lazy.Value;

        public ICommand Get(CommandType type, int intParam = 0)
        {
            var guid = Guid.NewGuid().ToString();
            if (type == CommandType.ExtractData)
            {
                if (intParam != 0)
                {
                    return new ExtractDataCommand(intParam)
                    {
                        Id = guid,
                        Status = Status.New,
                    };
                }
                else
                {
                    return new ExtractDataCommand()
                    {
                        Id = guid,
                        Status = Status.New,
                    };
                }
            }
            else if (type == CommandType.GatherData)
            {
                if (intParam != 0)
                {
                    return new GatherDataCommand(intParam)
                    {
                        Id = guid,
                        Status = Status.New,
                    };
                }
                else
                {
                    return new GatherDataCommand()
                    {
                        Id = guid,
                        Status = Status.New,
                    };
                }
            }

            throw new ArgumentException("Couldn't resolve dependency for given CommandType");
        }

        public ICommand Get(CommandType type, OfferType offerType)
        {
            var guid = Guid.NewGuid().ToString();
            if (type == CommandType.GetLinks)
            {
                return new GetLinksCommand(offerType)
                {
                    Id = guid,
                    Status = Status.New,
                };
            }

            throw new ArgumentException("Couldn't resolve dependency for given CommandType");
        }
    }
}