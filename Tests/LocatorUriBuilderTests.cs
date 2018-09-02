using MarklogicDataLayer.DataStructs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using WebAPI.Utils;

namespace Tests
{

    [TestClass]
    public class LocatorUriBuilderTests
    {
        [TestMethod]
        public void BuildUris_returns_properly_structured_uris()
        {
            var inputOffers = new List<Offer>
            {
                new Offer
                {
                    Area = "Wrzeszcz",
                    District = "Wrzeszcz",
                },
                new Offer
                {
                    Area = "Manifestu Polanieckiego 30",
                    District = "Wrzeszcz",
                },
            };
            var inputPOIs = new List<PointOfInterest>
            {
                new PointOfInterest
                {
                    Address = "aleja Grunwaldzka 141",
                    Name = "Galeria Baltycka",
                },
                new PointOfInterest
                {
                    Address = "Trubadurow 2",
                    Name = "Surf Burger",
                },
            };

            var expected = new List<string>
            {
                "https://maps.googleapis.com/maps/api/distancematrix/json?units=metric&origins=Wrzeszcz&destinations=Galeria Baltycka,aleja Grunwaldzka 141|Surf Burger,Trubadurow 2&key=AIzaSyCOSgW_ndo0wm22a1Ncgv260jWgaW2cfnQ",
                "https://maps.googleapis.com/maps/api/distancematrix/json?units=metric&origins=Manifestu Polanieckiego 30&destinations=Galeria Baltycka,aleja Grunwaldzka 141|Surf Burger,Trubadurow 2&key=AIzaSyCOSgW_ndo0wm22a1Ncgv260jWgaW2cfnQ",
            };
            var result = LocatorUriBuilder.BuildUris(inputOffers, inputPOIs);

            CollectionAssert.AreEquivalent(expected, result);
        }
    }
}
