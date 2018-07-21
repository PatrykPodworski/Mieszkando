using System;
using System.Collections.Generic;
using System.Text;

namespace MarklogicDataLayer.XQuery.Functions
{
    public class XdmpLockForUpdate : Function
    {
        private readonly MlUri _uri;

        public XdmpLockForUpdate(MlUri uri)
        {
            _uri = uri;
        }

        public override string Query => FunctionToQuery("xdmp:lock-for-update", _uri.Query);
    }
}
