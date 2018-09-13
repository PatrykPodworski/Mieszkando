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
        [XmlElement(CommandConstants.CommandId)]
        public string Id { get; set; }

        [XmlElement(CommandConstants.CreationDate)]
        public DateTime DateOfCreation { get; set; }

        [XmlElement(CommandConstants.LastModified)]
        public DateTime LastModified { get; set; }

        [XmlElement(CommandConstants.Status)]
        public Status Status { get; set; }

        public BaseCommand()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string GetId()
        {
            return Id;
        }

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

        public void SetDateOfCreation(DateTime date)
        {
            DateOfCreation = date;
        }

        public void SetLastModifiedDate(DateTime date)
        {
            LastModified = date;
        }
    }
}