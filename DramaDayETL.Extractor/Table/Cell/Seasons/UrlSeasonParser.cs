using DramaDayETL.Core.Abtraction;
using DramaDayETL.Extractor.Table.Cell.Abtraction;
using HtmlAgilityPack;

namespace DramaDayETL.Extractor.Table.Cell.Seasons
{
    internal class UrlSeasonParser : IParser<HtmlNode, Result<Season>>,
        IValidator<HtmlNode, Result>
    {
        public static Result Validate(HtmlNode input)
        {
            var hasUrlMetaNode = input.GetAttributeValue("property", string.Empty) == "og:url";

            if (!hasUrlMetaNode)
                return Result.Failure(new Error("UrlSeasonParser.ValidationFailed", "The meta tag with the title not found"));

            return Result.Success();
        }

        public static Result<Season> Parse(HtmlNode input)
        {
            var urlMetaNode = input.OwnerDocument.DocumentNode.SelectSingleNode(@"//meta[@property = ""og:url""]");

            var url = urlMetaNode.GetAttributeValue("content", string.Empty);

            var season = GetDramaSeason(url);

            return new Season { SeasonNumber = season };
        }

        public static Result<Season> ValidateAndParse(HtmlNode input)
        {
            return ParserWithValidation<HtmlNode, Season>.ParseWithValidation(
                input,
                Validate,
                Parse
            );
        }

        private static int GetDramaSeason(string url)
        {
            int season = 1;

            string[] titleParts = url.Split("-");
            string lastPart = titleParts[titleParts.Length - 1].Trim('/');

            if (lastPart.Length == 1 &&
                int.TryParse(lastPart, out int result) &&
                result > 1 && result < 7)
            {
                season = result;
            }

            return season;
        }
    }
}
