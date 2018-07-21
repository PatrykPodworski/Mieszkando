using System;
using System.Collections.Generic;
using System.Text;

namespace MarklogicDataLayer.XQuery.Functions
{
    public class FnDoc : Function
    {
        public static FnDoc AllDocuments = new FnDoc();
        readonly MlUri[] _uris;

        private FnDoc()
        {

        }

        public FnDoc(string uri)
            : this(new MlUri(uri, MlUriDocumentType.Xml))
        {

        }

        public FnDoc(MlUri uri)
            : this(new[] { uri })
        {

        }

        public FnDoc(MlUri[] uris)
        {
            _uris = uris;
        }


        public override string Query => FunctionToQuery("fn:doc", _uris == null ? string.Empty : new Sequence(_uris).Query);
    }
}
