using System;
using System.Collections.Generic;
using System.Text;

namespace MarklogicDataLayer.SearchQuery.SearchModels
{
    public class OfferSearchModel
    {
        public List<CostSearchModel> CostSearch { get; set; }
        public List<AreaSearchModel> AreaSearch { get; set; }

        public OfferSearchModel(List<CostSearchModel> costSearch, List<AreaSearchModel> areaSearch)
        {
            CostSearch = costSearch;
            AreaSearch = areaSearch;
        }
    }
}
