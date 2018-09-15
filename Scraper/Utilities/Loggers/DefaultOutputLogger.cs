using System;

namespace OfferScraper.Utilities.Loggers
{
    public class DefaultOutputLogger : ILogger
    {
        private string _source;

        public void Log(LogType type, string messsage)
        {
            System.Console.WriteLine($"{DateTime.Now.ToString()}|{type}|{_source}: {messsage}");
        }

        public void SetSource(string source)
        {
            _source = source;
        }
    }
}