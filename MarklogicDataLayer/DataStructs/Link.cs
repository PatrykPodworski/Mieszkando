namespace MarklogicDataLayer.DataStructs
{
    public class Link
    {
        public string Id { get; }
        public string Uri { get; }
        public OfferType LinkSourceKind { get; }

        public Link(string id, string uri, OfferType linkKind)
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
