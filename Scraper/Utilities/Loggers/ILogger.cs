namespace OfferScraper.Utilities.Loggers
{
    public interface ILogger
    {
        void Log(LogType type, string messsage);

        void SetSource(string source);
    }
}