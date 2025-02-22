using DramaDayETL.Extractor.Table.Cell.LinksGroup;

namespace DramaDayETL.Extractor.Table.Cell.EpisodeVersion
{
    public class EpVersion
    {
        public int Id { get; set; }
        public string EpisodeVerisonName { get; set; }

        public ICollection<ShortLink> Links { get; set; } = new List<ShortLink>();
    }
}
