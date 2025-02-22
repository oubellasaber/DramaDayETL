namespace DramaDayETL.Core.Abtraction
{
    public record Error(string Code, string Name)
    {
        public static Error None = new(string.Empty, string.Empty);
        public static Error NullValue = new("Error.NullValue", "Null value was provided");
        public static Error NoSuitableParserFound = new Error("Error.NoSuitableParserFound", "No suitable parser found for the provided input.");
        public static Error HtmlNodeIsNotTr = new("Error.HtmlNodeIsNotTr", "The provided HTML node is not a <tr> element");
        public static Error RowStructureIsNotSupported = new("Error.RowStructureIsNotSupported", "<tr> element has unexpected number of <td> elements");
        public static Error MismatchedParser = new("Error.MismatchedParser", "The provided HTML node does not match the expected format for the parser.");
        public static Error NotExpectedFormat = new("Error.NotExpectedFormat", "the <td> element is not in the expected Format");
        public static Error NoItemInPipelineMatched = new Error("Pipeline.NoItemInPipelineMatched", "None of the available items in the pipeline matched the input");
        public static Error UnsupportedUrl = new("URL.Unsupported", "The provided URL format is not supported.");
        public static Error InvalidHtml = new("HTML.Invalid", "Failed to parse HTML content.");
        public static Error ScraperNotFound = new("Scraper.NotFound", "No suitable scraper found for the URL.");
        public static Error ScrapingFailed = new("Scraping.Failed", "Failed to extract content from the page.");
        public static Error LinkParsingFailed = new("Links.ParsingFailed", "Failed to parse links from content.");
        public static Error TransformationFailed = new("Transform.Failed", "Failed to transform links to episodes.");
    }
}
