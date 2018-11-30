using Common.Models;
using MarklogicDataLayer.DataStructs;

namespace TomtomApiWrapper.Interafaces
{
    public interface ITomtomApi
    {
        Address ReverseGeocoding(string latitude, string longitude);

        CoordinatesResponse Geocoding(string address);
    }
}