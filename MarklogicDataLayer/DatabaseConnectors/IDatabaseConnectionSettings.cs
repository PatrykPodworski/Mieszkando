﻿namespace MarklogicDataLayer.DatabaseConnectors
{
    public interface IDatabaseConnectionSettings
    {
        string Host { get; }
        int Port { get; }
        string UserName { get; }
        string Password { get; }
    }
}