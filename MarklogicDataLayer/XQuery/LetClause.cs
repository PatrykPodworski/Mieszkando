using System;
using System.Collections.Generic;
using System.Text;

namespace MarklogicDataLayer.XQuery
{
    public class LetClause : Expression
    {
        private readonly VariableName _variableName;
        private readonly string _exp;

        public LetClause(VariableName variableName, Expression exp)
        {
            _variableName = variableName;
            _exp = exp.Query;
        }
        public override string Query => $"let {_variableName.Query} := {_exp}";
    }
}
