using MarklogicDataLayer.Constants;
using MarklogicDataLayer.SearchQuery.Providers;
using MarklogicDataLayer.SearchQuery.SearchModels;
using MarklogicDataLayer.XQuery.Constants;
using MarklogicDataLayer.XQuery.Functions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Tests
{
    [TestClass]
    [TestCategory("Unit")]
    public class OfferSearchQueryProviderTests
    {
        [TestMethod]
        public void GetSearchQuery_returns_proper_query_for_simple_model()
        {
            var searchModel = new OfferSearchModel(new List<(string TotalCost, string ComparisonOperator)>
            {
                ("1000", ComparisonOperators.Equal),
            }, new List<(string Area, string ComparisonOperator)>
            {
                ("43", ComparisonOperators.Equal),
            });
            var sut = new OfferSearchQueryProvider(searchModel);

            var expected = new CtsAndQuery(
                        new CtsElementRangeQuery(OfferConstants.TotalCost, ComparisonOperators.Equal, "1000"),
                        new CtsElementRangeQuery(OfferConstants.Area, ComparisonOperators.Equal, "43"),
                        new CtsCollectionQuery(OfferConstants.CollectionName)).Query;
            var actual = sut.GetSearchQuery();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetSearchQuery_returns_proper_query_for_complex_model()
        {
            var searchModel = new OfferSearchModel(new List<(string TotalCost, string ComparisonOperator)>
            {
                ("1000", ComparisonOperators.GreaterThan),
                ("1500", ComparisonOperators.LesserOrEqual),
            }, new List<(string Area, string ComparisonOperator)>
            {
                ("31", ComparisonOperators.GreaterThan),
                ("42", ComparisonOperators.LesserThan),
            });
            var sut = new OfferSearchQueryProvider(searchModel);

            var expected = new CtsAndQuery(
                        new CtsElementRangeQuery(OfferConstants.TotalCost, ComparisonOperators.GreaterThan, "1000"),
                        new CtsElementRangeQuery(OfferConstants.TotalCost, ComparisonOperators.LesserOrEqual, "1500"),
                        new CtsElementRangeQuery(OfferConstants.Area, ComparisonOperators.GreaterThan, "31"),
                        new CtsElementRangeQuery(OfferConstants.Area, ComparisonOperators.LesserThan, "42"),
                        new CtsCollectionQuery(OfferConstants.CollectionName)).Query;
            var actual = sut.GetSearchQuery();

            Assert.AreEqual(expected, actual);
        }
    }
}