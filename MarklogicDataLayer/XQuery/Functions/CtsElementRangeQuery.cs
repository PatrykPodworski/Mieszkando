using System;
using System.Collections.Generic;
using System.Text;

namespace MarklogicDataLayer.XQuery.Functions
{
    public class CtsElementRangeQuery : Function
    {
        private readonly string _elementName;
        private readonly string _operator;
        private readonly string _value;

        public CtsElementRangeQuery(string elementName, string op, string value)
        {
            _elementName = elementName;
            _operator = op;
            _value = value;
        }

        public override string Query
        {
            get
            {
                return FunctionToQuery(
                    "cts:element-range-query",
                    new XsQName(_elementName).Query,
                    _operator,
                    _value
                );
            }
        }
    }
}
