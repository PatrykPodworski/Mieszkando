using MarklogicDataLayer.DataStructs;

namespace OfferScraper.Commands.Interfaces
{
    public interface ICommand
    {
        bool IsNew();

        bool IsInProgress();

        void SetStatus(Status status);
    }
}