using System;

namespace MarklogicDataLayer.SearchQuery.SearchModels
{
    public class SimpleSearchModel
    {
        public string MaxCost { get; set; }
        public string NoOfRooms { get; set; }

        public SimpleSearchModel(string maxCost, string noOfRooms)
        {
            if (string.IsNullOrEmpty(maxCost) || string.IsNullOrEmpty(noOfRooms))
            {
                throw new ArgumentException();
            }
            MaxCost = maxCost;
            NoOfRooms = noOfRooms;
        }
    }
}
