using System;
using System.Collections.Generic;
using System.Text;

namespace MarklogicDataLayer.XQuery.Functions
{
    public class XsQName : Function
    {
        readonly string _localName;

        public XsQName(string localName)
        {
            _localName = localName;
        }

        public override string Query
        {
            get
            {
                return FunctionToQuery(
                    "xs:QName",
                    new Literal(_localName).Query);
            }
        }
    }
}
