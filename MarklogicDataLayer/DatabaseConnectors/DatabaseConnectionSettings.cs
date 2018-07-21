using System;
using System.Configuration;

namespace OfferLinkScraper.DatabaseConnectors
{
    public class DatabaseConnectionSettings : IDatabaseConnectionSettings
    {
        private readonly string _key;
        private string _host;
        private int _port;
        private string _userName;
        private string _password;

        public DatabaseConnectionSettings(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("key");

            _key = key;
        }

        public DatabaseConnectionSettings(string host, int port, string userName, string passWord)
        {
            _host = host;
            _port = port;
            _userName = userName;
            _password = passWord;
        }

        public string Host
        {
            get
            {
                LazyInit();
                return _host;
            }
        }

        public int Port
        {
            get
            {
                LazyInit();
                return _port;
            }
        }
        public string UserName
        {
            get
            {
                LazyInit();
                return _userName;
            }
        }
        public string Password
        {
            get
            {
                LazyInit();
                return _password;
            }
        }

        private void LazyInit()
        {
            if (_host != null)
                return;

            var connStr = ConfigurationManager.AppSettings.Get(_key);
            var parts = connStr.Split(':');
            if (parts.Length != 4)
                throw new InvalidOperationException("Invalid config value");

            _host = parts[0];
            _port = int.Parse(parts[1]);
            _userName = parts[2];
            _password = parts[3];
        }
    }
}