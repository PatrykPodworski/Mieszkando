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
    }
}