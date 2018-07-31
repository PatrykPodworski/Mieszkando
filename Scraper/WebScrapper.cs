using OfferScraper.Commands.Interfaces;

namespace OfferScraper
{
    public class WebScrapper
    {
        private readonly ICommandQueue _commandQueue;
        private readonly ICommandBus _commandBus;

        public WebScrapper()
        {
        }

        public void Run()
        {
            while (_commandQueue.HasNext())
            {
                var command = _commandQueue.GetNext();
                _commandBus.Send(command);
            }
        }
    }
}