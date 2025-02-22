using DramaDayETL.Core.Abtraction;
using DramaDayETL.Transformer.FileCryptExtractor.Entities.FileCryptContainer;

namespace DramaDayETL.Orchestrator
{
    public class Orchestrator
    {
        private readonly FileCryptContainerParsingService _fileCryptContainerParsingService;

        public Orchestrator(FileCryptContainerParsingService fileCryptContainerParsingService)
        {
            _fileCryptContainerParsingService = fileCryptContainerParsingService;
        }

        public async Result<Task> ExecuteAsync(Uri url)
        {
            string expectedHost = "dramaday.me";

            if (url.Host.Equals(expectedHost, StringComparison.OrdinalIgnoreCase) ||
                !string.IsNullOrEmpty(url.Query))
            {
                return Result.Failure<Task>(
                    new Error("Orchestrator.InvalidUrl", "The provided URL is invalid.")
                );
            }

            // the crawler for getting html is not implemented yet

            // var media = MediaParser.Parse(html);
            
            // load raw data into db

            // Get the filecrypt link using L4SResolver

            // call the FileCryptParsingService to get the FileCryptContainer

            // Group the files by name

            // Get filename metadata

            // call the get download link on each group

            // get metadata using ffprobe

            // load data to database

            // wait for new data
        }
    }
}
