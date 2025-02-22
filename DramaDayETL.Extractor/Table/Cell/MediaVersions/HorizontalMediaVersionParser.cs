using DramaDayETL.Core.Abtraction;
using DramaDayETL.Extractor.Table.Cell.Abtraction;
using HtmlAgilityPack;

namespace DramaDayETL.Extractor.Table.Cell.MediaVersions
{
    internal class HorizontalMediaVersionParser : IParser<HtmlNode, Result<MediaVersion>>,
        IValidator<HtmlNode, Result>
    {
        public static Result Validate(HtmlNode input)
        {
            var tdNodes = input.SelectNodes(".//td")?.ToList();

            var validationResult = tdNodes?.Count switch
            {
                3 => ValidateThreeCellVersion(tdNodes),
                2 => ValidateTwoCellVersion(tdNodes),
                _ => false
            };

            if (!validationResult)
                return Result.Failure(Error.MismatchedParser);

            return Result.Success();
        }

        private static bool ValidateThreeCellVersion(IReadOnlyList<HtmlNode> tdNodes)
        {
            return !string.IsNullOrWhiteSpace(tdNodes[0].InnerText) && // First cell has content
                   string.IsNullOrWhiteSpace(tdNodes[1].InnerText) &&  // Second cell is empty
                   string.IsNullOrWhiteSpace(tdNodes[2].InnerText);    // Third cell is empty
        }

        private static bool ValidateTwoCellVersion(IReadOnlyList<HtmlNode> tdNodes)
        {
            return string.IsNullOrWhiteSpace(tdNodes[0].InnerText) && // First cell is empty
                   !string.IsNullOrWhiteSpace(tdNodes[1].InnerText);  // Second cell has content
        }

        public static Result<MediaVersion> Parse(HtmlNode input)
        {
            var mediaVersion = new MediaVersion();

            var tds = input.SelectNodes("./td");
            int tdCount = tds.Count;

            if (tdCount == 2)
                mediaVersion.MediaVersionName = input.SelectSingleNode("./td[2]").InnerText;

            if (tdCount == 3)
                mediaVersion.MediaVersionName = input.SelectSingleNode("./td[1]").InnerText;

            return mediaVersion;
        }

        public static Result<MediaVersion> ValidateAndParse(HtmlNode input)
        {
            return ParserWithValidation<HtmlNode, MediaVersion>.ParseWithValidation(
                input,
                Validate,
                Parse
            );
        }
    }
}
