using System;
using System.Collections.Generic;
using System.Text;

namespace MarklogicDataLayer.SearchQuery.SearchModels
{
    public class AreaSearchModel
    {
        public AreaSearchModel(string totalArea, string comparisonOperator)
        {
            TotalArea = totalArea;
            ComparisonOperator = comparisonOperator;
        }

        public string TotalArea { get; set; }
        public string ComparisonOperator { get; set; }
    }
}
