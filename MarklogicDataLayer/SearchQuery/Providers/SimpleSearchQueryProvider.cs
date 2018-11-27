using MarklogicDataLayer.Constants;
using MarklogicDataLayer.SearchQuery.SearchModels;
using MarklogicDataLayer.XQuery;
using MarklogicDataLayer.XQuery.Functions;
using System.Collections.Generic;

namespace MarklogicDataLayer.SearchQuery.Providers
{
    public class SimpleSearchQueryProvider
    {
        private readonly SimpleSearchModel _searchModel;

        public SimpleSearchQueryProvider(SimpleSearchModel searchModel)
        {
            _searchModel = searchModel;
        }

        public Expression GetSearchExpression()
        {
            var subQueries = new List<Function>();

            subQueries.Add(new CtsElementRangeQuery(OfferConstants.TotalCost, "'<='", _searchModel.MaxCost));
            subQueries.Add(new CtsElementValueQuery(OfferConstants.Rooms, _searchModel.NoOfRooms));
            subQueries.Add(new CtsCollectionQuery(OfferConstants.CollectionName));

            var result = new CtsAndQuery(subQueries);

            return result;
        }
    }
}
