using System.Collections.Generic;
using TomtomApiWrapper.Models;

namespace TomtomApiWrapper.Responses
{
    public class ReverseGeocodingResponse
    {
        public Summary Summary { get; set; }
        public List<AddressResponse> Addresses { get; set; }
    }

    public class Summary
    {
        public int QueryTime { get; set; }
        public int NumResults { get; set; }
    }

    public class AddressResponse
    {
        public Address Address { get; set; }
        public string Position { get; set; }
    }
}