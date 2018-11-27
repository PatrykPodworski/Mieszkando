namespace WebAPI.Models
{
    public class OfferModel
    {
        public string Title { get; set; }
        public decimal TotalCost { get; set; }
        public string District { get; set; }
        public double Area { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}