using MarklogicDataLayer.DataStructs;
using MarklogicDataLayer.Repositories;
using MarklogicDataLayer.SearchQuery.Providers;
using OfferSearcher.Extensions;
using OfferSearcher.Interfaces;
using OfferSearcher.Models;
using OfferSearcher.SearchCriteria;
using RouteFinders.Interfaces;
using System.Collections.Generic;
using System.Linq;
using TomtomApiWrapper.Interafaces;

namespace OfferSearcher.Services
{
    public class OfferSearchService : IOfferSearchService
    {
        private readonly IDataRepository<Offer> _repository;
        private readonly ITomtomApi _tomtomApi;
        private readonly IRouteFinder _routeFinder;

        public OfferSearchService(IDataRepository<Offer> repository, ITomtomApi tomtomApi,
            IRouteFinder routeFinder)
        {
            _repository = repository;
            _tomtomApi = tomtomApi;
            _routeFinder = routeFinder;
        }

        public ICollection<OfferModel> AdvancedSearch(AdvancedSearchCriteria criteria)
        {
            throw new System.NotImplementedException();
        }

        public ICollection<OfferModel> SimpleSearch(SimpleSearchCriteria criteria)
        {
            var searchModel = criteria.MapToSearchModel();

            var queryProvider = new SimpleSearchQueryProvider(searchModel);
            var query = queryProvider.GetSearchExpression();
            var result = _repository.GetWithExpression(query, 1000, 1);

            var offerModels = result
                .Select(x => x.MapToOfferModel())
                .ToList();

            return offerModels;
        }
    }
}