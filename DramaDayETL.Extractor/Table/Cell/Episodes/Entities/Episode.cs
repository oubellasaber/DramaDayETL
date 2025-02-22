using DramaDayETL.Extractor.Table.Cell.EpisodeVersion;

namespace DramaDayETL.Extractor.Table.Cell.Episodes.Entities
{
    public class Episode
    {
        public int Id { get; set; }
        public ICollection<EpVersion> EpisodeVersions { get; set; } = new List<EpVersion>();
    }
}
