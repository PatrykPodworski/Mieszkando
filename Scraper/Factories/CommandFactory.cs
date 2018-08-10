﻿using OfferScraper.Commands.Interfaces;
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

        public ICommand Get(CommandType type, int intParam = 0)
        {
            if (type == CommandType.ExtractData)
            {
                if (intParam != 0)
                {
                    return new ExtractDataCommand(intParam)
                    {
                        DateOfCreation = DateTime.Now,
                        LastModified = DateTime.Now,
                        Status = Status.New,
                    };
                }
                else
                {
                    return new ExtractDataCommand()
                    {
                        DateOfCreation = DateTime.Now,
                        LastModified = DateTime.Now,
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
                        DateOfCreation = DateTime.Now,
                        LastModified = DateTime.Now,
                        Status = Status.New,
                    };
                }
                else
                {
                    return new GatherDataCommand()
                    {
                        DateOfCreation = DateTime.Now,
                        LastModified = DateTime.Now,
                        Status = Status.New,
                    };
                }
            }

            throw new ArgumentException("Couldn't resolve dependency for given CommandType");
        }

        public ICommand Get(CommandType type, OfferType offerType)
        {
            if (type == CommandType.GetLinks)
            {
                return new GetLinksCommand(offerType)
                {
                    DateOfCreation = DateTime.Now,
                    LastModified = DateTime.Now,
                    Status = Status.New,
                };
            }

            throw new ArgumentException("Couldn't resolve dependency for given CommandType");
        }
    }
}
