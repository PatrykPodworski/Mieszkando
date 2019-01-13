using MarklogicDataLayer.Constants;
using MarklogicDataLayer.SearchQuery.SearchModels;
using MarklogicDataLayer.XQuery;
using MarklogicDataLayer.XQuery.Functions;
using System.Collections.Generic;

namespace MarklogicDataLayer.SearchQuery.Providers
{
    public class SimpleSearchQueryProvider
    {
        private readonly SearchModel _searchModel;

        public SimpleSearchQueryProvider(SearchModel searchModel)
        {
            _searchModel = searchModel;
        }

        public Expression GetSearchExpression()
        {
            var subQueries = new List<Function>();

            subQueries.Add(new CtsElementRangeQuery(OfferConstants.TotalCost, "'<='", new XsDouble(_searchModel.MaxCost).Query));
            subQueries.Add(new CtsElementRangeQuery(OfferConstants.Rooms, "'>='", new XsInteger(_searchModel.NumberOfRooms).Query));
            subQueries.Add(new CtsNotQuery(new CtsElementValueQuery(OfferConstants.OfferType, OfferTypeConstants.Outdated)));
            subQueries.Add(new CtsCollectionQuery(OfferConstants.CollectionName));

            var result = new CtsAndQuery(subQueries);

            return result;
        }
    }
}
