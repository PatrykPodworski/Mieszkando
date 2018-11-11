using System;
using System.Collections.Generic;
using System.Text;

namespace MarklogicDataLayer.SearchQuery.SearchModels
{
    public class CostSearchModel
    {
        public CostSearchModel(string totalCost, string comparisonOperator)
        {
            TotalCost = totalCost;
            ComparisonOperator = comparisonOperator;
        }

        public string TotalCost { get; set; }
        public string ComparisonOperator { get; set; }
    }
}
