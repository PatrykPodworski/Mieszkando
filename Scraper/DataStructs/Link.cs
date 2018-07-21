namespace OfferScrapper.DataStructs
{
    public class Link
    {
        public string Id { get; }
        public string Uri { get; }
        public LinkKind LinkSourceKind { get; }

        public Link(string id, string uri, LinkKind linkKind)
        {
            Id = id;
            Uri = uri;
            LinkSourceKind = linkKind;
        }

        public override string ToString()
        {
            return $"{Id}|{Uri}";
        }
    }
}
