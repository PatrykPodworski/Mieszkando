using System.ComponentModel.DataAnnotations;

namespace OfferSearcher.SearchCriteria
{
    public class SimpleSearchCriteria
    {
        [Required]
        public int MaxCost { get; set; }

        [Required]
        public int NumberOfRooms { get; set; }
    }
}