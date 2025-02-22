using DramaDayETL.Core.Abtraction;
using DramaDayETL.Extractor.Table.Cell.Abtraction;
using DramaDayETL.Extractor.Table.Cell.Validators;
using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace DramaDayETL.Extractor.Table.Cell.Seasons
{
    internal class HorizontalSeasonParser : IParser<HtmlNode, Result<Season>>,
        IValidator<HtmlNode, Result>
    {
        public static Result Validate(HtmlNode input)
        {
            var cellValidationResult = BaseHtmlTableCellValidator.Validate(input);
            if (cellValidationResult.IsFailure)
                return cellValidationResult;

            var tdNodes = input.SelectNodes(".//td");

            var firstCellText = tdNodes[0].InnerText;
            if (!(Regex.IsMatch(firstCellText, @"season (\d+)", RegexOptions.IgnoreCase) &&
                string.IsNullOrEmpty(tdNodes[1].InnerText)))
                return Result.Failure(Error.MismatchedParser);

            return Result.Success();
        }

        public static Result<Season> Parse(HtmlNode input)
        {
            var match = Regex.Match(
                input.SelectSingleNode("./td[1]").InnerText,
                @"season (\d+)",
                RegexOptions.IgnoreCase
            );

            var season = new Season
            {
                SeasonNumber = match.Success ? int.Parse(match.Groups[1].Value) : null
            };

            return Result.Success(season);
        }

        public static Result<Season> ValidateAndParse(HtmlNode input)
        {
            return ParserWithValidation<HtmlNode, Season>.ParseWithValidation(
                input,
                Validate,
                Parse
            );
        }
    }
}
