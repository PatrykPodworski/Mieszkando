using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarklogicDataLayer.XQuery
{
    public class MultiStatement : Expression
    {
        private readonly List<Expression> _innerExpressions = new List<Expression>();

        private static readonly string Separator = $";{Environment.NewLine}";

        public MultiStatement()
        {

        }

        public MultiStatement(IEnumerable<Expression> expressions)
        {
            foreach (var expression in expressions)
            {
                Add(expression);
            }
        }

        public void Add(Expression expression)
        {
            _innerExpressions.Add(expression);
        }

        public override string Query => string.Join(Separator, _innerExpressions.Select(x => x.Query));
    }
}
