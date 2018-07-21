using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace MarklogicDataLayer.XQuery
{
    public class ReturnClause : Expression
    {
        readonly string _arg;

        public ReturnClause(Expression expression)
        {
            _arg = expression.Query;
        }

        public ReturnClause(XElement xmlStructure)
        {
            _arg = xmlStructure.ToString();
        }

        public override string Query => $"return {_arg}";
    }
}
