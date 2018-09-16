using MarklogicDataLayer.DataStructs;
using OfferScraper.Commands.Implementation;
using OfferScraper.Commands.Interfaces;
using System;

namespace CommandAdder
{
    public class CommandsDatabaseInserter : IAdder
    {
        private ICommandQueue _commandQueue;

        public CommandsDatabaseInserter(ICommandQueue commandQueue)
        {
            _commandQueue = commandQueue;
        }

        public void Add(string[] names)
        {
            if (names.Length == 0)
            {
                AddSpecificCommands(names);
                return;
            }

            AddDefaultCommands();
        }

        private void AddSpecificCommands(string[] names)
        {
            throw new NotImplementedException();
        }

        private void AddDefaultCommands()
        {
            _commandQueue.Add(new GetLinksCommand(OfferType.Olx));
            _commandQueue.Add(new GetLinksCommand(OfferType.OtoDom));
        }
    }
}