using System.ComponentModel.DataAnnotations;

namespace MarklogicDataLayer.SearchQuery.SearchModels
{
    public class SimpleSearchModel
    {
        [Required]
        public string MaxCost { get; set; }

        [Required]
        public string NumberOfRooms { get; set; }
    }
}