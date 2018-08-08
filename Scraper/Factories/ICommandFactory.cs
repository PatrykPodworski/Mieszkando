using MarklogicDataLayer.DataStructs;
using System;
using System.Collections.Generic;
using System.Text;

namespace OfferScraper.Factories
{
    public interface ICommandFactory<T>
    {
        T Get(CommandType type, int numberOfLinks);
        T Get(CommandType type, OfferType offerType);
    }
}
