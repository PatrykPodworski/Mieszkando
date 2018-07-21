using System;
using System.Collections.Generic;
using System.Text;

namespace MarklogicDataLayer.XQuery
{
    public abstract class Expression
    {
        public abstract string Query
        {
            get;
        }

        public virtual string RawQuery => Query;
    }
}
