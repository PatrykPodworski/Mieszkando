using System.Collections.Generic;

namespace WebAPI.Models
{
    public class GroupedOffersModel
    {
        public string District { get; set; }
        public ICollection<OfferModel> Offers { get; set; }
    }
}