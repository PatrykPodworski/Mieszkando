﻿using System.Collections.Generic;

namespace TomtomApiWrapper.Responses
{
    public class ReverseGeocodingResponse
    {
        public Summary Summary { get; set; }
        public List<AddressResponse> Addresses { get; set; }
    }
}