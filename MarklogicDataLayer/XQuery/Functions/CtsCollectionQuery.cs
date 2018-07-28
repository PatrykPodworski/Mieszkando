using System.Linq;
using System.Text;

namespace MarklogicDataLayer.XQuery.Functions
{
    public class CtsCollectionQuery : Function
    {
        private readonly string _collectionNames;

        public override string Query => FunctionToQuery("cts:collection-query", _collectionNames);

        public CtsCollectionQuery(string collectionName)
        {
            _collectionNames = new Literal(collectionName).Query;
        }
        public CtsCollectionQuery(string[] collectionNames)
        {
            var qb = new StringBuilder();
            qb.Append('(').Append(string.Join(",", collectionNames.Select(x => new Literal(x).Query))).Append(')');
            _collectionNames = qb.ToString();
        }
    }
}
