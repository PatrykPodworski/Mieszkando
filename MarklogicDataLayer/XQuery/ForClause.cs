using System;
using System.Collections.Generic;
using System.Text;

namespace MarklogicDataLayer.XQuery
{
    public class ForClause : Expression
    {
        private readonly VariableName _itemName;
        private readonly Expression _expression;


        public ForClause(VariableName itemName, Expression expression)
        {
            _itemName = itemName;
            _expression = expression;
        }

        public override string Query => $"for {_itemName.Query} in {_expression.Query}";

    }
}
