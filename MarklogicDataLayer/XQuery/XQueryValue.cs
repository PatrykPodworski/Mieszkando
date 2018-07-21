using System;
using System.Collections.Generic;
using System.Text;

namespace MarklogicDataLayer.XQuery
{
    public abstract class XQueryValue : Expression
    {
        public Literal ToLiteral()
        {
            return new Literal(this.FormatToText());
        }

        public virtual string FormatToText() => Query;
    }
}
