using MarklogicDataLayer.Constants;
using MarklogicDataLayer.SearchQuery.SearchModels;
using MarklogicDataLayer.XQuery;
using MarklogicDataLayer.XQuery.Functions;
using System.Collections.Generic;

namespace MarklogicDataLayer.SearchQuery.Providers
{
    public class AdvancedSearchQueryProvider
    {
        private readonly SearchModel _searchModel;

        public AdvancedSearchQueryProvider(SearchModel searchModel)
        {
            _searchModel = searchModel;
        }

        public Expression GetSearchExpression()
        {
            var subQueries = new List<Function>();

            double.TryParse(_searchModel.NumberOfRooms, out var rooms);
            subQueries.Add(new CtsElementRangeQuery(OfferConstants.TotalCost, "'<='", new XsDouble(_searchModel.MaxCost).Query));
            subQueries.Add(new CtsElementRangeQuery(OfferConstants.TotalCost, "'>='", new XsDouble(_searchModel.MinCost).Query));
            subQueries.Add(new CtsElementRangeQuery(OfferConstants.Area, "'<='", new XsDouble(_searchModel.MaxArea).Query));
            subQueries.Add(new CtsElementRangeQuery(OfferConstants.Area, "'>='", new XsDouble(_searchModel.MinArea).Query));
            subQueries.Add(new CtsElementValueQuery(OfferConstants.Rooms, rooms));
            subQueries.Add(new CtsCollectionQuery(OfferConstants.CollectionName));

            var result = new CtsAndQuery(subQueries);

            return result;
        }
    }
}
