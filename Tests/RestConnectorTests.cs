using MarklogicDataLayer;
using MarklogicDataLayer.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OfferLinkScraper.DatabaseConnectors;

namespace UnitTests
{
    [TestClass]
    public class RestConnectorTests
    {
        [TestInitialize]
        public void Initialize()
        {
            DbConnectionSettings = new DatabaseConnectionSettings("vps561493.ovh.net", 8086, "admin", "admin");
        }

        [TestCleanup]
        public void Cleanup()
        {
            DbFlush.Perform(DbConnectionSettings);
        }

        protected IDatabaseConnectionSettings DbConnectionSettings;

        [TestMethod]
        public void Submits_query_without_errors()
        {
            var documentName = "test_01.xml";
            var content = "test_01_value";
            var query = $"xquery version '1.0-ml'; " +
                        $"xdmp:document-insert('{documentName}', {GetTestDocument(documentName, content)})";

            var rest = new RestConnector(DbConnectionSettings);
            var response = rest.Submit(query);

            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        private static string GetTestDocument(string documentName, string content)
        {
            return $"<link documentName=\"{documentName}\">" +
                   $"{content}</link>";
        }
    }
}
