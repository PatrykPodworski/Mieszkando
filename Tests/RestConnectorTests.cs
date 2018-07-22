using MarklogicDataLayer;
using MarklogicDataLayer.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OfferLinkScraper.DatabaseConnectors;
using System.Net.Http;

namespace UnitTests
{
    [TestCategory("Integration")]
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

        [TestMethod]
        public void Commit_Marklogic_content_transaction_changes_document()
        {
            var documentName = "test_01";
            var content = "test_01_value";
            var doc = MarklogicContent.Xml(documentName, GetTestDocument(documentName, content));

            var rest = new RestConnector(DbConnectionSettings);
            rest.Insert(doc);

            var transaction = rest.BeginTransaction();

            doc = MarklogicContent.Xml(documentName, GetTestDocument(documentName, $"{content}_changed"));
            rest.Insert(doc, transaction);
            rest.CommitTransaction(transaction);

            var query = $"xquery version '1.0-ml'; fn:doc('/{documentName}.xml')";
            var response = rest.Submit(query);
            var actual = response.Content.ReadAsMultipartAsync().Result.Contents[0].ReadAsStringAsync().Result;
            var expected = MarklogicContent.Xml(documentName, GetTestDocumentXml(documentName, $"{content}_changed")).Content;
            Assert.IsTrue(actual.Equals(expected));
        }

        [TestMethod]
        public void Rollback_Marklogic_content_transaction_does_not_change_document()
        {
            var documentName = "test_02";
            var content = "test_02_value";
            var doc = MarklogicContent.Xml(documentName, GetTestDocument(documentName, content));

            var rest = new RestConnector(DbConnectionSettings);
            rest.Insert(doc);

            var transaction = rest.BeginTransaction();

            doc = MarklogicContent.Xml(documentName, GetTestDocument(documentName, $"{content}_changed"));
            rest.Insert(doc, transaction);
            rest.RollbackTransaction(transaction);

            var query = $"xquery version '1.0-ml'; fn:doc('/{documentName}.xml')";
            var response = rest.Submit(query);
            var actual = response.Content.ReadAsMultipartAsync().Result.Contents[0].ReadAsStringAsync().Result;
            var expected = MarklogicContent.Xml(documentName, GetTestDocumentXml(documentName, content)).Content;
            Assert.IsTrue(actual.Equals(expected));
        }

        private static string GetTestDocument(string documentName, string content)
        {
            return $"<link documentName=\"{documentName}\">" +
                   $"{content}</link>";
        }

        private static string GetTestDocumentXml(string documentName, string content)
        {
            return $"<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n{GetTestDocument(documentName, content)}";
        }
    }
}
