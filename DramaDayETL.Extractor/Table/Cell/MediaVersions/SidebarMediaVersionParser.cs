using DramaDayETL.Core.Abtraction;
using DramaDayETL.Extractor.Table.Cell.Abtraction;
using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace DramaDayETL.Extractor.Table.Cell.MediaVersions
{
    internal class SidebarMediaVersionParser : IParser<HtmlNode, Result<MediaVersion>>,
        IValidator<HtmlNode, Result>
    {
        public static Result Validate(HtmlNode input)
        {
            var tdNodes = input.SelectNodes(".//td");

            var validationResult = Regex.IsMatch(tdNodes[0].InnerText, @"^\d{1,2}-\d{1,2}\s+(.+)$", RegexOptions.Singleline);

            if (!validationResult)
                return Result.Failure(Error.MismatchedParser);

            return Result.Success();
        }

        public static Result<MediaVersion> Parse(HtmlNode input)
        {
            return new MediaVersion
            {
                MediaVersionName = Regex.Match(input.SelectSingleNode("./td[1]").InnerText,
                                                  @"^\d{1,2}-\d{1,2}\s+(.+)$",
                                                  RegexOptions.Singleline).Groups[1].Value
            };
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
