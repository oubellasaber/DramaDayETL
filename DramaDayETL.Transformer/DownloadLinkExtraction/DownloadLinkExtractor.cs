using DramaDayETL.Core.Abtraction;

namespace DramaDayETL.Transformer.DownloadLinkExtraction
{
    internal class DownloadLinkExtractor
    {
        private static readonly Dictionary<string, IDownloadLinkExtractor> _supportedDownloadServices =
            new Dictionary<string, IDownloadLinkExtractor>();

        public DownloadLinkExtractor(PixelDrainDownloadLinkExtractor pixelDrainDownloadLinkExtractor,
                                   DataNodesDownloadLinkExtractor dataNodesDownloadLinkExtractor)
        {
            _supportedDownloadServices.Add("pixeldrain.com", pixelDrainDownloadLinkExtractor);
            _supportedDownloadServices.Add("datanodes.to", dataNodesDownloadLinkExtractor);
        }

        public async Task<Result<Uri>> ExtractDownloadLinkAsync(IEnumerable<Uri> links)
        {
            foreach (var link in links)
            {
                if (!_supportedDownloadServices.TryGetValue(link.Host, out var extractor))
                {
                    continue;
                }

                var downloadLinkResult = await extractor.GetDownloadUrlAsync(link);
                if (downloadLinkResult.IsSuccess)
                {
                    return downloadLinkResult;
                }
            }

            return Result.Failure<Uri>(new Error("DownloadLinkExtractor.NotFound", "No valid download link found"));
        }
    }
}