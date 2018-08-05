using MarklogicDataLayer.DataStructs;
using OfferScraper.Commands.Interfaces;
using System;

namespace OfferScraper.Commands.Implementation
{
    public abstract class BaseCommand : ICommand
    {
        public DateTime DateOfCreation { get; set; }
        public DateTime LastModified { get; set; }
        public Status Status { get; set; }

        public bool IsNew()
        {
            return Status == Status.New;
        }
    }
}