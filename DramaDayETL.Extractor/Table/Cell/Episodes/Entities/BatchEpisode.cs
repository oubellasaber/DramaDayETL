namespace DramaDayETL.Extractor.Table.Cell.Episodes.Entities
{
    internal class BatchEpisode : Episode
    {
        public int RangeStart { get; set; }
        public int RangeEnd { get; set; }
    }
}
