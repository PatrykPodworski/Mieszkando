﻿namespace OfferScraper.Commands.Interfaces
{
    public interface ICommandBus
    {
        void Send(ICommand command);
    }
}