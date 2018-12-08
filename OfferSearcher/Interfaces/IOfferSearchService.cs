using OfferSearcher.Models;
using OfferSearcher.SearchCriteria;
using System.Collections.Generic;

namespace OfferSearcher.Interfaces
{
    public interface IOfferSearchService
    {
        ICollection<OfferModel> SimpleSearch(SimpleSearchCriteria model);

        (ICollection<OfferModel>, ICollection<PointOfInterest>) AdvancedSearch(AdvancedSearchCriteria model);
    }
}