using MarklogicDataLayer.XQuery;
using MarklogicDataLayer.XQuery.Functions;
using OfferLinkScraper.DatabaseConnectors;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarklogicDataLayer.Utility
{
    public static class DbFlush
    {
        public static void Perform(IDatabaseConnectionSettings connectionSettings)
        {
            var rest = new RestConnector(connectionSettings);
            var flwor = new Flwor(new XdmpNodeDelete(FnDoc.AllDocuments.Query));

            rest.Submit(flwor.Query);
        }
    }
}
