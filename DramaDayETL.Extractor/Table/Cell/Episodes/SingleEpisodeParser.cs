using DramaDayETL.Core.Abtraction;
using DramaDayETL.Extractor.Table.Cell.Abtraction;
using DramaDayETL.Extractor.Table.Cell.Episodes.Entities;
using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace DramaDayETL.Extractor.Table.Cell.Episodes
{
    internal class SingleEpisodeParser : IParser<HtmlNode, Result<SingleEpisode>>,
        IValidator<HtmlNode, Result>
    {
        public static Result Validate(HtmlNode input)
        {
            var isSingle = Regex.IsMatch(
                input.SelectSingleNode("./td[1]").InnerText,
                @"^\s*\d{1,2}\s*(?:\n.*)?$",
                RegexOptions.Singleline);

            if (!isSingle)
                return Result.Failure(Error.MismatchedParser);

            return Result.Success();
        }

        public static Result<SingleEpisode> Parse(HtmlNode input)
        {
            return new SingleEpisode
            {
                EpisodeNumber = int.Parse(
                    input.SelectSingleNode("./td[1]")
                    .InnerText
                    .Substring(0, Math.Min(2, input.SelectSingleNode("./td[1]").InnerText.Length))
                )
            };
        }

        public static Result<SingleEpisode> ValidateAndParse(HtmlNode input)
        {
            return ParserWithValidation<HtmlNode, SingleEpisode>.ParseWithValidation(
                input,
                Validate,
                Parse
            );
        }
    }
}
