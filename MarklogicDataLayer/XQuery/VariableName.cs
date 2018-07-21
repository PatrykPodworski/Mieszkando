using System;
using System.Collections.Generic;
using System.Text;

namespace MarklogicDataLayer.XQuery
{
    public class VariableName : Expression
    {
        private readonly string _value;

        public VariableName(string value)
        {
            _value = value;
        }

        public override string Query => $"${_value}";
    }
}
