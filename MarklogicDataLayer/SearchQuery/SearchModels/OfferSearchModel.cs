using System;
using System.Collections.Generic;
using System.Text;

namespace MarklogicDataLayer.SearchQuery.SearchModels
{
    public class OfferSearchModel
    {
        public List<(string TotalCost, string ComparisonOperator)> CostSearch { get; set; }
        public List<(string Area, string ComparisonOperator)> AreaSearch { get; set; }

        public OfferSearchModel(List<(string TotalCost, string ComparisonOperator)> costSearch, List<(string Area, string ComparisonOperator)> areaSearch)
        {
            CostSearch = costSearch;
            AreaSearch = areaSearch;
        }
    }
}
