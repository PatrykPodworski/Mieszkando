using System;
using System.Collections.Generic;
using System.Text;

namespace MarklogicDataLayer.XQuery.Functions
{
    public class CtsSearch : Function
    {
        public static readonly Sequence DefaultOptions = new Sequence(new Literal("unfiltered"));

        readonly string _node;
        readonly Expression _query;
        readonly Sequence _options;

        public CtsSearch(string node, Function query, Sequence options = null)
        {
            _node = node;
            _query = query;
            _options = options ?? new Sequence();
        }

        public CtsSearch(Function node, Function query, Sequence options = null)
        {
            _node = node.Query;
            _query = query;
            _options = options ?? new Sequence();
        }

        public CtsSearch(string node, Expression query, Sequence options = null)
        {
            _node = node;
            _query = query;
            _options = options ?? new Sequence();
        }

        public CtsSearch(Function node, Expression query, Sequence options = null)
        {
            _node = node.Query;
            _query = query;
            _options = options ?? new Sequence();
        }

        public override string Query
        {
            get
            {
                var sb = new StringBuilder();

                sb.Append(FunctionToQuery(
                    "cts:search",
                    _node,
                    _query.Query,
                    _options.Query));

                return sb.ToString();
            }
        }
    }
}
