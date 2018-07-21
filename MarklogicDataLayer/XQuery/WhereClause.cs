using System;
using System.Collections.Generic;
using System.Text;

namespace MarklogicDataLayer.XQuery
{
    public class WhereClause : Expression {
        private readonly string _arg;

    public WhereClause(string expression)
    {
        _arg = $"{expression}";
    }

    public override string Query => $"where {_arg}";
}
}
