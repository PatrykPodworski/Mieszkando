using System;
using System.Collections.Generic;
using System.Text;

namespace MarklogicDataLayer.XQuery.Functions
{
    public class XdmpDocumentDelete : Function
    {
        private readonly MlUri _uri;

        public XdmpDocumentDelete(MlUri uri)
        {
            _uri = uri;
        }

        public override string Query => FunctionToQuery("xdmp:document-delete", _uri.Query);
    }
}
