﻿using MarklogicDataLayer.Constants;
using MarklogicDataLayer.SearchQuery.Providers;
using MarklogicDataLayer.SearchQuery.SearchModels;
using MarklogicDataLayer.XQuery.Functions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    [TestCategory("Unit")]
    public class SimpleSearchQueryProviderTests
    {
        [TestMethod]
        public void GetSearchExpression_returns_correct_query_for_simple_search()
        {
            var searchModel = new SearchModel { MaxCost = "1000", NumberOfRooms = "1" };

            var sut = new SimpleSearchQueryProvider(searchModel);
            var expected = new CtsAndQuery(
                new CtsElementRangeQuery(OfferConstants.TotalCost, "'<='", new XsDouble(searchModel.MaxCost).Query),
                new CtsElementRangeQuery(OfferConstants.Rooms, "'>='", new XsInteger(searchModel.NumberOfRooms).Query),
                new CtsNotQuery(new CtsElementValueQuery(OfferConstants.OfferType, OfferTypeConstants.Outdated)),
            new CtsCollectionQuery(OfferConstants.CollectionName)).Query;

            var result = sut.GetSearchExpression().Query;

            Assert.AreEqual(expected, result);
        }
    }
}