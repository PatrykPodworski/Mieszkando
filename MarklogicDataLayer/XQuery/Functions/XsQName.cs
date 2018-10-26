using System;
using System.Collections.Generic;
using System.Text;

namespace MarklogicDataLayer.XQuery.Functions
{
    public class XsQName : Function
    {
        readonly string _localName;
        readonly string _namespace;

        public XsQName(string localName, string ns = null)
        {
            _localName = localName;
            _namespace = ns ?? string.Empty;
        }

        public override string Query
        {
            get
            {
                return FunctionToQuery(
                    "xs:QName",
                    new Literal(_namespace).Query,
                    new Literal(_localName).Query);
            }
        }
    }
}
