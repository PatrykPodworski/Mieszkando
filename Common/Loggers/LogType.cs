using Common.Extensions;

namespace Common.Loggers
{
    public enum LogType
    {
        [StringValue("INFO")]
        Info,

        [StringValue("ERROR")]
        Error,

        [StringValue("DEBUG")]
        Debug
    }
}