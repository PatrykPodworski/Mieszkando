using System;
using System.Collections.Generic;
using System.Text;

namespace MarklogicDataLayer.XQuery.Functions
{
    public class XdmpNodeDelete : Function
    {
        readonly string _node;

        public XdmpNodeDelete(string node)
        {
            _node = node;
        }

        public override string Query
        {
            get
            {
                return FunctionToQuery("xdmp:node-delete", _node);
            }
        }
    }
}
