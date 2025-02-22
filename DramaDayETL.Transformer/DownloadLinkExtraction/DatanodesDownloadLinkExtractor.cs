using DramaDayETL.Core.Abtraction;
using System.Net;
using System.Text.RegularExpressions;

namespace DramaDayETL.Transformer.DownloadLinkExtraction
{
    public class DataNodesDownloadLinkExtractor : IDownloadLinkExtractor
    {
        private static readonly Regex DataNodesPattern = new(
            @"datanodes\.to/([a-zA-Z0-9]+)",
            RegexOptions.Compiled | RegexOptions.IgnoreCase
        );

        private readonly HttpClient _client;

        public DataNodesDownloadLinkExtractor(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient("NoAutoRedirect");
        }

        public bool CanHandleUrlAsync(Uri url)
        {
            string urlString = url.ToString();
            Match match = DataNodesPattern.Match(urlString);

            return match.Success;
        }

        public async ValueTask<Result<Uri>> GetDownloadUrlAsync(Uri url)
        {
            var validationResult = CanHandleUrlAsync(url);
            if (validationResult)
            {
                return Result.Failure<Uri>(
                    new Error("DownloadLinkExtractor.NotSupported", "URL does not match rthe expected format")
                    );
            }

            string fileId = url.Segments[^1];

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "https://datanodes.to/download");
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("op", "download2"),
                new KeyValuePair<string, string>("id", fileId)
            });

            requestMessage.Content = content;

            try
            {
                var response = await _client.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead);

                if (response.StatusCode == HttpStatusCode.Found && response.Headers.Location != null)
                {
                    return response.Headers.Location;
                }

                return Result.Failure<Uri>(new Error("DownloadLinkExtractor.DownloadFailed",
                    $"Failed to get download URL."));
            }
            catch (HttpRequestException)
            {
                return Result.Failure<Uri>(new Error("DownloadLinkExtractor.NetworkError",
                    $"Network error while fetching download URL"));
            }
        }
    }
}
