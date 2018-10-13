
using MarklogicDataLayer.DataStructs;

namespace TomtomApiWrapper.Interafaces
{
    public interface ITomtomApi
    {
        Address ReverseGeocoding(string latitude, string longitude);
    }
}