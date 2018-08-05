using System;
using System.Collections.Generic;
using System.Text;

namespace MarklogicDataLayer.XQuery
{
    public class EmptyExpression : Expression
    {
        public override string Query => "()";
    }
}
