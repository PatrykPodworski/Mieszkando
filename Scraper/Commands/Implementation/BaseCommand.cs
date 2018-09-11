using MarklogicDataLayer.DataStructs;
using OfferScraper.Commands.Interfaces;
using OfferScraper.Constants;
using System;
using System.Linq;
using System.Xml.Serialization;

namespace OfferScraper.Commands.Implementation
{
    public abstract class BaseCommand : ICommand
    {
        [XmlElement(CommandConstants.CreationDate)]
        public DateTime DateOfCreation { get; set; }

        [XmlElement(CommandConstants.LastModified)]
        public DateTime LastModified { get; set; }

        [XmlElement(CommandConstants.Status)]
        public Status Status { get; set; }

        public bool IsNew()
        {
            return Status == Status.New;
        }

        public bool IsInProgress()
        {
            return Status == Status.InProgress;
        }

        public void SetStatus(Status status)
        {
            Status = status;
        }

        public void CheckCommandType(ICommand command)
        {
            if (GetCommandType() != command.GetType())
            {
                throw new ArgumentException($"Given command type is invalid. Expected: {GetCommandType()}, actual {command.GetType()}.");
            }
        }

        public Type GetCommandType()
        {
            return GetType()
                .GetInterfaces()
                .FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(ICommandHandler<>))
                .GetGenericArguments()[0];
        }
    }
}