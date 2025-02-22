using DramaDayETL.Transformer.FileCryptExtractor.Services.RowParsingService;

namespace DramaDayETL.Transformer.FileCryptExtractor.HttpClients;

public class FileCryptClient : IFileCryptClient
{
    private readonly HttpClient _httpClient;
    private readonly ScraperApiClient _scraperApiClient;

    public FileCryptClient(
        IHttpClientFactory httpClientFactory,
        ScraperApiClient scraperApiClient)
    {
        _httpClient = httpClientFactory.CreateClient("Default");
        _scraperApiClient = scraperApiClient;
    }

    public async Task<string> GetAsync(string url, StringContent? content)
    {
        try
        {
            // First attempt with regular HttpClient
            var response = await (content is null ?
                _httpClient.GetAsync(url) :
                _httpClient.PostAsync(url, content));

            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(responseBody))
            {
                throw new HttpRequestException("Received empty response from server");
            }

            // If no captcha, return the response
            if (!ContainsCaptcha(responseBody))
            {
                return responseBody;
            }

            var scraperResponse = await _scraperApiClient.GetAsync(url, content);
            scraperResponse.EnsureSuccessStatusCode();

            var scraperResponseBody = await scraperResponse.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(scraperResponseBody))
            {
                throw new HttpRequestException("Received empty response from ScraperAPI");
            }

            return scraperResponseBody;
        }
        catch (HttpRequestException ex)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage httpRequestMessage)
    {
        return await _httpClient.SendAsync(httpRequestMessage);
    }

    public async Task<HttpResponseMessage> GetHeadersAsync(Uri url)
        => await _httpClient.GetAsync(url);

    private bool ContainsCaptcha(string responseBody)
    => responseBody.Contains("Please confirm that you are no robot", StringComparison.OrdinalIgnoreCase);
}