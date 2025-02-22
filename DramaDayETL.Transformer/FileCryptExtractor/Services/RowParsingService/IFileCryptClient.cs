namespace DramaDayETL.Transformer.FileCryptExtractor.Services.RowParsingService;
public interface IFileCryptClient
{
    Task<HttpResponseMessage> GetHeadersAsync(Uri url);
    Task<string> GetAsync(string url, StringContent? content);
    Task<HttpResponseMessage> SendAsync(HttpRequestMessage httpRequestMessage);
}