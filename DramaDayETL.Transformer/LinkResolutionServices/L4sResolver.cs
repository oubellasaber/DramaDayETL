using System.Text;
using System.Web;

namespace DramaDayETL.Transformer.LinkResolutionServices
{
    public class L4sResolver
    {
        private readonly HttpClient _client;

        public L4sResolver(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient("NoAutoRedirect");
        }

        public async Task<string?> ResolveLink(string link)
        {
            var response = await _client.GetAsync(link);

            if (response == null)
                return null;

            string? url = response?.Headers?.Location.ToString();

            if (url == null)
                return null;

            return GetFinalUrlAsync(url);
        }

        public static string? ExtractBase64String(string url)
        {

            string? base64String = ExtractAfterTr(url);

            if (string.IsNullOrEmpty(base64String))
            {
                Console.WriteLine("No url query found in the URL");
                return null;
            }

            byte[] decodedBytes = Convert.FromBase64String(base64String);
            string decodedString = Encoding.UTF8.GetString(decodedBytes);

            return decodedString;

            string? ExtractAfterTr(string url)
            {
                Uri uri = new Uri(url);

                var path = uri.AbsolutePath;
                const string prefix = "/tr/";

                if (path.Contains(prefix))
                    return path.Substring(path.IndexOf(prefix) + prefix.Length);

                return null;
            }
        }

        public static string? GetFinalUrlAsync(string url)
        {
            try
            {
                Uri uri = new Uri(url);
                string? finalUrl = null;

                var queryParams = HttpUtility.ParseQueryString(uri.Query);
                string? encodedUrl = queryParams["url"] ?? queryParams["s"];

                if (!string.IsNullOrEmpty(encodedUrl))
                {
                    if (!IsBase64String(encodedUrl))
                        return encodedUrl;

                    string preFinalUrl = DecodeBase64(encodedUrl);

                    if (Uri.TryCreate(preFinalUrl, UriKind.Absolute, out Uri? uriResult) &&
                        string.IsNullOrEmpty(uriResult?.Query))
                    {
                        return preFinalUrl;
                    }

                    Console.WriteLine($"Decoded string {preFinalUrl} is not a valid URL.");
                }

                return finalUrl;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }


        private static string DecodeBase64(string base64String)
        {
            try
            {
                byte[] bytes = Convert.FromBase64String(base64String);
                return Encoding.UTF8.GetString(bytes);
            }
            catch (FormatException)
            {
                throw new ArgumentException("Invalid base64 string.");
            }
        }

        private static bool IsBase64String(string str)
        {
            Span<byte> buffer = new Span<byte>(new byte[str.Length]);
            return Convert.TryFromBase64String(str, buffer, out _);
        }
    }
}
