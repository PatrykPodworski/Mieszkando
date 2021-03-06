﻿using Common.Models;
using MarklogicDataLayer.DataStructs;
using RestSharp;
using System.Linq;
using TomtomApiWrapper.Interafaces;
using TomtomApiWrapper.Responses;

namespace TomtomApiWrapper
{
    public class TomtomApi : ITomtomApi
    {
        private string _apiKey;
        private IRestClient _restClient;

        public TomtomApi(string apiKey, IRestClient restClient)
        {
            _apiKey = apiKey;
            _restClient = restClient;
        }

        public Address ReverseGeocoding(string latitude, string longitude)
        {
            var apiMethod = "reverseGeocode";

            var request = new RestRequest
            {
                Resource = $"{apiMethod}/{latitude},{longitude}.json?key={_apiKey}"
            };

            var response = _restClient.Execute<ReverseGeocodingResponse>(request);

            return response.Data.Addresses
                .Select(x => x.Address)
                .FirstOrDefault();
        }

        public CoordinatesResponse Geocoding(string address)
        {
            var apiMethod = "geocode";
            var limit = 1;
            var countrySet = "PL";
            var topLeft = "54.27%2C18.43%20";
            var bottomRight = "54.45%2C18.95";

            var request = new RestRequest
            {
                Resource = "{apiMethod}/{address}.json"
            };

            request.AddUrlSegment("apiMethod", apiMethod);
            request.AddUrlSegment("address", address);
            request.AddQueryParameter("limit", limit.ToString());
            request.AddQueryParameter("countrySet", countrySet);
            request.AddQueryParameter("topLeft", topLeft);
            request.AddQueryParameter("bottomRight", bottomRight);
            request.AddQueryParameter("key", _apiKey);

            var response = _restClient.Execute<GeocodingResponse>(request);

            return response.Data.Results
                .Select(x => x.Position)
                .FirstOrDefault();
        }
    }
}