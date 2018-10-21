using System;
using System.Collections.Generic;
using System.Text;

namespace MarklogicDataLayer.XQuery.Functions
{
    public class FnSubsequence : Function
    {
        private readonly Expression _expression;
        private readonly long _numberOfDocuments;
        private readonly long _startFrom;

        public FnSubsequence(Expression expression, long numberOfDocuments = long.MinValue)
        {
            _expression = expression;
            _numberOfDocuments = numberOfDocuments;
        }

        public FnSubsequence(Expression expression, long numberOfDocuments = long.MinValue, long startFrom = 1)
        {
            _expression = expression;
            _numberOfDocuments = numberOfDocuments;
            _startFrom = startFrom;
        }

        public override string Query => _numberOfDocuments == long.MinValue
            ? FunctionToQuery("fn:subsequence", _expression.Query, _startFrom.ToString())
            : FunctionToQuery("fn:subsequence", _expression.Query, _startFrom.ToString(), _numberOfDocuments.ToString());
    }
}
