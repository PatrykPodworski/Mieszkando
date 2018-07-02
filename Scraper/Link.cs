namespace OfferLinkScraper
{
    public class Link
    {
        public string Id { get; }
        public string Uri { get; }

        public Link(string id, string uri)
        {
            Id = id;
            Uri = uri;
        }

        public override string ToString()
        {
            return $"{Id}|{Uri}";
        }
    }
}
