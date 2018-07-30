using System;
using System.Collections.Generic;
using System.Text;

namespace MarklogicDataLayer.XQuery.Functions
{
    public class FnQName : Function
    {
        readonly string _localName;
        readonly string _namespace;

        public FnQName(string localName, string ns = null)
        {
            _localName = localName;
            _namespace = ns ?? string.Empty;
        }

        public override string Query
        {
            get
            {
                return FunctionToQuery(
                    "fn:QName",
                    new Literal(_namespace).Query,
                    new Literal(_localName).Query);
            }
        }
    }
}
