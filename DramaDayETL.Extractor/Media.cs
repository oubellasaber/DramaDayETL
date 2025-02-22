using DramaDayETL.Extractor.Table.Cell.Seasons;

namespace DramaDayETL.Extractor
{
    public class Media
    {
        public string DramaDayId { get; set; }
        public string? KrTitle { get; set; }
        public string EnTitle { get; set; }
        public IEnumerable<Season> Seasons { get; set; } = new List<Season>();
    }
}