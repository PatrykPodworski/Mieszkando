using MarklogicDataLayer.Constants;
using MarklogicDataLayer.SearchQuery.SearchModels;
using MarklogicDataLayer.XQuery;
using MarklogicDataLayer.XQuery.Functions;
using System.Collections.Generic;

namespace MarklogicDataLayer.SearchQuery.Providers
{
    public class OfferSearchQueryProvider
    {
        private readonly OfferSearchModel _searchModel;

        public OfferSearchQueryProvider(OfferSearchModel searchModel)
        {
            _searchModel = searchModel;
        }

        public Expression GetSearchExpression()
        {
            var subQueries = new List<Function>();

            foreach (var costSearch in _searchModel.CostSearch)
            {
                subQueries.Add(new CtsElementRangeQuery(OfferConstants.TotalCost, costSearch.ComparisonOperator, costSearch.TotalCost));
            }

            foreach (var areaSearch in _searchModel.AreaSearch)
            {
                subQueries.Add(new CtsElementRangeQuery(OfferConstants.Area, areaSearch.ComparisonOperator, areaSearch.TotalArea));
            }

            subQueries.Add(new CtsCollectionQuery(OfferConstants.CollectionName));

            var result = new CtsAndQuery(subQueries);

            return result;
        }
    }
}
