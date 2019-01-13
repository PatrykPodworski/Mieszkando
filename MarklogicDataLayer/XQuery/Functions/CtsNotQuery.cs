using System;
using System.Collections.Generic;
using System.Text;

namespace MarklogicDataLayer.XQuery.Functions
{
    public class CtsNotQuery : Function
    {
        readonly List<Function> _queries;

        Sequence QuerySequence => new Sequence(_queries.ToArray());

        public CtsNotQuery(params Function[] queries)
        {
            _queries = new List<Function>(queries);
        }
        public CtsNotQuery(List<Function> queries)
        {
            _queries = queries;
        }

        public override string Query => FunctionToQuery("cts:not-query", QuerySequence.Query);
    }
}
