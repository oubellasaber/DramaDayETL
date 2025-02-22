using DramaDayETL.Core.Abtraction;
using DramaDayETL.Extractor.Table.Cell.Abtraction;
using DramaDayETL.Extractor.Table.Cell.Episodes.Entities;
using HtmlAgilityPack;

namespace DramaDayETL.Extractor.Table.Cell.Episodes
{
    internal class SpecialEpisodeParser : IParser<HtmlNode, Result<SpecialEpisode>>,
        IValidator<HtmlNode, Result>
    {
        public static Result Validate(HtmlNode input)
        {
            var isSpecial = input.SelectSingleNode("./td[1]")
                   .InnerText
                   .Contains("special", StringComparison.OrdinalIgnoreCase);

            if (!isSpecial)
                return Result.Failure(Error.MismatchedParser);

            return Result.Success();
        }

        public static Result<SpecialEpisode> Parse(HtmlNode input)
        {
            return new SpecialEpisode
            {
                Title = input.SelectSingleNode("./td[1]").InnerText
            };
        }

        public static Result<SpecialEpisode> ValidateAndParse(HtmlNode input)
        {
            return ParserWithValidation<HtmlNode, SpecialEpisode>.ParseWithValidation(
                input,
                Validate,
                Parse
            );
        }
    }
}
