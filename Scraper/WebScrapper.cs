using OfferScraper.Commands.Implementation;
using OfferScraper.Commands.Interfaces;
using OfferScraper.Utilities.Loggers;
using System.Threading;

namespace OfferScraper
{
    public class WebScrapper
    {
        private readonly ICommandQueue _commandQueue;
        private readonly ILogger _logger;
        private readonly int _sleepDuration;
        private readonly ICommandBus _commandBus;

        public WebScrapper(ICommandBus commandBus, ICommandQueue commandQueue, ILogger logger)
        {
            _commandBus = commandBus;
            _commandQueue = commandQueue;

            _logger = logger;
            logger.SetSource(typeof(WebScrapper).Name);

            _sleepDuration = 10 * 60000;    // configuration
        }

        public void Run()
        {
            while (true)
            {
                _logger.Log(LogType.Info, "Program started");
                
                try
                {
                    while (_commandQueue.HasNext())
                    {
                        var command = _commandQueue.GetNext();
                        
                        _commandBus.Send(command);
                    }
                }
                catch (System.Exception)
                {
                    throw;
                }

                _logger.Log(LogType.Info, $"No more commands to handle, sleeping for {_sleepDuration / 60000} minutes");
                Thread.Sleep(_sleepDuration);
            }
        }
    }
}