using DramaDayETL.Core.Abtraction;
using HtmlAgilityPack;
using System.Web;

namespace DramaDayETL.Extractor.MediaInfo
{
    internal class DramaDayPostIdParser : IParser<HtmlNode, Result<string>>,
        IValidator<HtmlNode, Result>
    {
        public static Result Validate(HtmlNode input)
        {
            var hasIdUrl = input
                .SelectSingleNode(@"//link[@rel = ""shortlink""]")
                .GetAttributeValue("href", string.Empty) != default;

            if (!hasIdUrl)
                return Result.Failure(new Error("DramaDayPostIdParser.NotFound", "The link tag with the post postId not found"));

            return Result.Success();
        }

        public static Result<string> Parse(HtmlNode input)
        {
            var url = input
                .SelectSingleNode(@"//link[@rel = ""shortlink""]")
                .GetAttributeValue("href", string.Empty);

            var uri = new Uri(url);

            var queryParams = HttpUtility.ParseQueryString(uri.Query);
            string postId = queryParams["p"]!;

            return postId;
        }
    }
}
