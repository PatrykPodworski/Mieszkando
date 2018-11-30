using Common.Models;
using MarklogicDataLayer.DataStructs;

namespace TomtomApiWrapper.Interafaces
{
    public interface ITomtomApi
    {
        Address ReverseGeocoding(string latitude, string longitude);

        Coordinates Geocoding(string address);
    }
}