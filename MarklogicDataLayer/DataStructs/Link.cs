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

        public override bool Equals(object obj)
        {
            var item = obj as Link;
            if (item == null)
            {
                return false;
            }

            return this.Id == item.Id 
                && this.LinkSourceKind == item.LinkSourceKind 
                && this.Uri == item.Uri;
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode() ^ this.LinkSourceKind.GetHashCode() ^ this.Uri.GetHashCode();
        }
    }
}
