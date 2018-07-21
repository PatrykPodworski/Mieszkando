using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarklogicDataLayer.XQuery
{
    public class Sequence : Expression
    {
        readonly List<Expression> _elements;

        public Sequence(IEnumerable<string> strings)
            : this(strings.Select(x => new Literal(x)))
        {
        }

        public Sequence(params Expression[] elements)
        {
            _elements = new List<Expression>(elements);
        }

        public Sequence(IEnumerable<Expression> elements)
        {
            _elements = new List<Expression>(elements);
        }

        public override string Query
        {
            get
            {
                var qb = new StringBuilder("(");

                var idx = 0;
                foreach (var element in _elements)
                {
                    qb.Append(element.Query);
                    if (++idx < _elements.Count)
                    {
                        qb.Append(", ");
                    }
                }

                qb.Append(")");
                return qb.ToString();
            }
        }
        public override string RawQuery
        {
            get
            {
                var qb = new StringBuilder("(");

                var idx = 0;
                foreach (var element in _elements)
                {
                    qb.Append(element.RawQuery);
                    if (++idx < _elements.Count)
                    {
                        qb.Append(", ");
                    }
                }

                qb.Append(")");
                return qb.ToString();
            }
        }
    }
}
