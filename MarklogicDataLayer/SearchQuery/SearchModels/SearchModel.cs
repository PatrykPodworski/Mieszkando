using System.ComponentModel.DataAnnotations;

namespace MarklogicDataLayer.SearchQuery.SearchModels
{
    public class SearchModel
    {
        public string MinCost { get; set; }

        [Required]
        public string MaxCost { get; set; }

        [Required]
        public string NumberOfRooms { get; set; }

        public string MinArea { get; set; }

        public string MaxArea { get; set; }
    }
}