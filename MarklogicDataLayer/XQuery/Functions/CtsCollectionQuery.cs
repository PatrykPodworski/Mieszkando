using System;
using System.Collections.Generic;
using System.Text;

namespace MarklogicDataLayer.XQuery.Functions
{
    public class CtsCollectionQuery : Function
    {
        private readonly string[] _collections;

        public CtsCollectionQuery(string[] collections)
        {
            _collections = collections;
        }

        public override string Query => FunctionToQuery("cts:collection-query", _collections);
    }
}
