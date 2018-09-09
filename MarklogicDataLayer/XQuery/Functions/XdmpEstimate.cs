using System;
using System.Collections.Generic;
using System.Text;

namespace MarklogicDataLayer.XQuery.Functions
{
    public class XdmpEstimate : Function
    {
        private readonly string _expression;

        public override string Query => FunctionToQuery("xdmp:estimate", _expression);

        public XdmpEstimate(Function expression)
        {
            _expression = expression.Query;
        }
    }
}
