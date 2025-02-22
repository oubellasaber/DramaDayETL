using DramaDayETL.Core.Abtraction;
using HtmlAgilityPack;

namespace DramaDayETL.Extractor.MediaInfo
{
    public class EnglishTitleParser : IParser<HtmlNode, Result<string>>,
        IValidator<HtmlNode, Result>
    {
        public static Result Validate(HtmlNode input)
        {
            var hasContentAttr = input
                .SelectSingleNode(@"//meta[@property = ""og:url""]")
                .GetAttributeValue("content", string.Empty) != default;

            if (!hasContentAttr)
                return Result.Failure(new Error("EnglishTitleParser.NotFound", "The meta tag with the url not found"));

            return Result.Success();
        }

        public static Result<string> Parse(HtmlNode input)
        {
            var url = input
                .SelectSingleNode(@"//meta[@property = ""og:url""]")
                .GetAttributeValue("content", string.Empty);

            return GetDramaTitleFromUrl(url);
        }

        private static string GetDramaTitleFromUrl(string url)
        {
            var uri = new Uri(url);

            string title = uri.GetComponents(UriComponents.Path, UriFormat.SafeUnescaped)
                .Trim('/')
                .Replace('-', ' ');

            string titleWithoutSeasonData = RemoveSeasonData(title);

            return titleWithoutSeasonData;
        }

        private static string RemoveSeasonData(string title)
        {
            var titleParts = title.Split(' ').ToList();

            if (titleParts.Count > 2 &&
               titleParts[titleParts.Count - 2].Contains("season", StringComparison.OrdinalIgnoreCase))
            {
                titleParts.RemoveAt(titleParts.Count - 2);
            }

            if (titleParts.Count > 1 &&
                int.TryParse(titleParts[titleParts.Count - 1], out int season) &&
                season >= 1 && season < 7)
            {
                titleParts.RemoveAt(titleParts.Count - 1);
            }

            return string.Join(' ', titleParts);
        }
    }
}