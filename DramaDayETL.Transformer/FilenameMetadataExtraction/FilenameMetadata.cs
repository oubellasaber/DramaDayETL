namespace DramaDayETL.Transformer.FilenameMetadataExtraction
{
    public class FilenameMetadata
    {
        public string OriginalFilename { get; set; }
        public string? Quality { get; set; }
        public string? ReleaseGroup { get; set; }
        public string? Network { get; set; }
        public string? RipType { get; set; }
        public int? Season { get; set; }
        public int? Episode { get; set; }
    }
}
