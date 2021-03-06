﻿using MarklogicDataLayer.DataStructs;
using TomtomApiWrapper.Responses;

namespace TomtomApiWrapper.Interafaces
{
    public interface ITomtomApi
    {
        Address ReverseGeocoding(string latitude, string longitude);

        CoordinatesResponse Geocoding(string address);
    }
}