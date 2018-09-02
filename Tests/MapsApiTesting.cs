using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Tests
{
    [TestClass]
    public class MapsApiTesting
    {
        private const string _apiKey = "AIzaSyCOSgW_ndo0wm22a1Ncgv260jWgaW2cfnQ";
        private const string _unit = "metric";
        private const string _start = "Gdansk";
        private const string _destination = "Warsaw";
        private static readonly string _url = $"https://maps.googleapis.com/maps/api/distancematrix/json?units={_unit}&origins={_start}&destinations={_destination}&key={_apiKey}";

        [TestMethod]
        public void Test()
        {
            var request = WebRequest.Create(_url);
            var response = request.GetResponse();
            var data = response.GetResponseStream();
            var reader = new StreamReader(data);
            var responseString = reader.ReadToEnd();
            response.Close();

            Assert.IsTrue(true);
        }
    }
}
