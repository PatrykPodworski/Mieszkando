using System;
using System.Collections.Generic;
using System.Text;

namespace MarklogicDataLayer.XQuery.Functions
{
    public class CtsAndQuery : Function
    {
        readonly List<Function> _queries;

        Sequence QuerySequence => new Sequence(_queries.ToArray());

        public CtsAndQuery(params Function[] queries)
        {
            _queries = new List<Function>(queries);
        }
        public CtsAndQuery(List<Function> queries)
        {
            _queries = queries;
        }

        public override string Query => FunctionToQuery("cts:and-query", QuerySequence.Query);
    }
}
