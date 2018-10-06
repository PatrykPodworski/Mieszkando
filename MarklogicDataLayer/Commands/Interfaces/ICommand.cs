using MarklogicDataLayer.DataStructs;
using System;

namespace MarklogicDataLayer.Commands.Interfaces
{
    public interface ICommand
    {
        bool IsNew();

        bool IsInProgress();

        void SetStatus(Status status);

        void SetDateOfCreation(DateTime date);

        void SetLastModifiedDate(DateTime date);

        string GetId();
    }
}