using System;
using System.Collections.Generic;
using System.Text;

namespace MarklogicDataLayer.XQuery
{
    public class Literal : XQueryValue
    {
        readonly string _value;

        public Literal(string value)
        {
            _value = value;
        }

        public override string Query
        {
            get
            {
                if (_value == null)
                    return null;

                var encoded = Utility.Encoder.Encode(_value);

                return $"'{encoded}'";
            }
        }

        public override string FormatToText() => _value;
    }
}
