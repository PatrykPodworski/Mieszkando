using MarklogicDataLayer.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace MarklogicDataLayer.SearchQuery.SearchModels
{
    [Serializable, XmlRoot(SearchModelConstants.SearchModelRoot)]
    public class SearchModel
    {
        [XmlElement(SearchModelConstants.Id)]
        public string Id { get; set; }
        [XmlElement(SearchModelConstants.MinCost)]
        public string MinCost { get; set; }

        [Required]
        [XmlElement(SearchModelConstants.MaxCost)]
        public string MaxCost { get; set; }

        [Required]
        [XmlElement(SearchModelConstants.NumberOfRooms)]
        public string NumberOfRooms { get; set; }

        [XmlElement(SearchModelConstants.MinArea)]
        public string MinArea { get; set; }

        [XmlElement(SearchModelConstants.MaxArea)]
        public string MaxArea { get; set; }

        public SearchModel()
        {
            Id = Guid.NewGuid().ToString();
        }

        public override bool Equals(object obj)
        {
            var model = obj as SearchModel;
            return model != null &&
                   Id == model.Id &&
                   MinCost == model.MinCost &&
                   MaxCost == model.MaxCost &&
                   NumberOfRooms == model.NumberOfRooms &&
                   MinArea == model.MinArea &&
                   MaxArea == model.MaxArea;
        }

        public override int GetHashCode()
        {
            var hashCode = -500806469;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Id);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(MinCost);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(MaxCost);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(NumberOfRooms);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(MinArea);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(MaxArea);
            return hashCode;
        }
    }
}