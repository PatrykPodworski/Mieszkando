using System;
using System.Collections.Generic;
using System.Text;

namespace MarklogicDataLayer.XQuery.Functions
{
    public abstract class Function : Expression
    {
        protected string FunctionToQuery(string functionName, params string[] args)
        {
            var qb = new StringBuilder();
            qb.Append(functionName);
            qb.Append("(");

            var idx = 0;
            foreach (var argument in args)
            {
                qb.Append(argument);
                if (++idx < args.Length)
                {
                    qb.Append(", ");
                }
            }

            qb.Append(")");

            return qb.ToString();
        }
    }
}
