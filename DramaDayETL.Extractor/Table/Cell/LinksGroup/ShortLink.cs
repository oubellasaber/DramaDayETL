namespace DramaDayETL.Extractor.Table.Cell.LinksGroup
{
    public class ShortLink
    {
        public int Id { get; set; }
        public string Host { get; set; }
        public string LinkUrl { get; set; }
        public string? DirectLink { get; set; }

        public override string ToString()
        {
            return $"Id: {Id}\nHost: {Host}\nLinkUrl: {LinkUrl}\nDirectLink: {DirectLink}";
        }
    }
}
