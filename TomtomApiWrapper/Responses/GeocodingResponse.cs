using System.Collections.Generic;

namespace TomtomApiWrapper.Responses
{
    public class GeocodingResponse
    {
        public Summary Summary { get; set; }
        public List<PositionResponse> Results { get; set; }
    }
}