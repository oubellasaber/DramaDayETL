using DramaDayETL.Core.Abtraction;
using System.Text.RegularExpressions;

namespace DramaDayETL.Transformer.DownloadLinkExtraction
{
    public class PixelDrainDownloadLinkExtractor : IDownloadLinkExtractor
    {
        private static readonly Regex PixelDrainPattern = new(
            @"pixeldrain\.com/u/([a-zA-Z0-9_-]+)",
            RegexOptions.Compiled | RegexOptions.IgnoreCase
        );

        public bool CanHandleUrlAsync(Uri url)
        {
            string urlString = url.ToString();
            Match match = PixelDrainPattern.Match(urlString);
            return match.Success;
        }

        public ValueTask<Result<Uri>> GetDownloadUrlAsync(Uri url)
        {
            var validationResult = CanHandleUrlAsync(url);
            if (validationResult)
            {
                return ValueTask.FromResult(Result.Failure<Uri>(
                    new Error("DownloadLinkExtractor.NotSupported", "URL does not match rthe expected format")
                    ));
            }

            var downloadUrl = new Uri($"https://pixeldrain.com/api/file/{url.Segments[2].TrimEnd('/')}?download");
            return ValueTask.FromResult(Result.Success(downloadUrl));
        }
    }
}