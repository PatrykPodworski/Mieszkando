using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace MarklogicDataLayer.XQuery.Functions
{
    public class CtsElementValueQuery : Function
    {
        private readonly string _elemName;
        private readonly Expression _values;
        private readonly string[] _options;
        private readonly double _weight;
        private readonly string _ns;


        public CtsElementValueQuery(string elemName, string value, string[] options = null, double weight = 1.0, string ns = null)
            : this(elemName, new Literal(value), options, weight, ns)
        {
        }

        public CtsElementValueQuery(string elemName, VariableName values, string[] options = null, double weight = 1.0, string ns = null)
            : this(elemName, options, weight, ns)
        {
            _values = values;
        }

        public CtsElementValueQuery(string elemName, IEnumerable<string> values, string[] options = null, double weight = 1.0, string ns = null)
            : this(elemName, values.Select(x => new Literal(x)).ToList(), options, weight, ns)
        {
        }

        public CtsElementValueQuery(string elemName, XQueryValue value, string[] options = null, double weight = 1.0, string ns = null)
            : this(elemName, new List<XQueryValue> { value }, options, weight, ns)
        {
        }

        public CtsElementValueQuery(string elemName, IEnumerable<XQueryValue> values, string[] options = null, double weight = 1.0, string ns = null)
            :this(elemName, options, weight, ns)
        {
            _values = new Sequence(values.Select(x => x.ToLiteral()));
        }

        private CtsElementValueQuery(string elemName, string[] options = null, double weight = 1.0, string ns = null)
        {
            _elemName = elemName;
            _options = options ?? new string[] { };
            _ns = ns ?? string.Empty;

            if (weight < -16 || weight > 64)
            {
                throw new ArgumentOutOfRangeException(nameof(weight), weight, " Valid values: [-16, 64]. ");
            }
            _weight = weight;
        }

        public override string Query
        {
            get
            {
                return FunctionToQuery(
                    "cts:element-value-query",
                    new FnQName(_elemName, _ns).Query,
                    _values.Query,
                    new Sequence(_options.Select(x => new Literal(x))).Query,
                    _weight.ToString(CultureInfo.InvariantCulture)
                );
            }
        }
    }
}
