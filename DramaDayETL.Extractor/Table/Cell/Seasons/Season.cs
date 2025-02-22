using DramaDayETL.Extractor.Table.Cell.MediaVersions;

namespace DramaDayETL.Extractor.Table.Cell.Seasons
{
    public class Season
    {
        public int Id { get; set; }
        public int? SeasonNumber { get; set; }

        public ICollection<MediaVersion> MediaVersions { get; set; } = new List<MediaVersion>();
    }
}
