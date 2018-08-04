using System;
using System.Collections.Generic;
using System.Text;

namespace MarklogicDataLayer.XQuery.Functions
{
    public class FnSubsequence : Function
    {
        private readonly Expression _expression;
        private readonly long _numberOfDocuments;

        public FnSubsequence(Expression expression, long numberOfDocuments = long.MinValue)
        {
            _expression = expression;
            _numberOfDocuments = numberOfDocuments;
        }

        public override string Query => _numberOfDocuments == long.MinValue
            ? FunctionToQuery("fn:subsequence", _expression.Query, "1")
            : FunctionToQuery("fn:subsequence", _expression.Query, "1", _numberOfDocuments.ToString());
    }
}
