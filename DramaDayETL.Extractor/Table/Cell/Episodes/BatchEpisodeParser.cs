using DramaDayETL.Core.Abtraction;
using DramaDayETL.Extractor.Table.Cell.Abtraction;
using DramaDayETL.Extractor.Table.Cell.Episodes.Entities;
using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace DramaDayETL.Extractor.Table.Cell.Episodes
{
    internal class BatchEpisodeParser : IParser<HtmlNode, Result<BatchEpisode>>,
        IValidator<HtmlNode, Result>
    {
        public static Result Validate(HtmlNode input)
        {
            var epCell = input.SelectSingleNode("./td[1]");

            if (!Regex.IsMatch(epCell.InnerText, @"\d{1,2}-\d{1,2}"))
                return Result.Failure(Error.MismatchedParser);

            return Result.Success();
        }

        public static Result<BatchEpisode> Parse(HtmlNode input)
        {
            BatchEpisode ep = new BatchEpisode();

            Regex rangedEpisodesReg = new Regex(@"(\d{1,2})-(\d{1,2})");
            var rangedEps = rangedEpisodesReg.Matches(input.SelectSingleNode("./td[1]").InnerText);

            var firstMatch = rangedEps[0];
            int leftEp = int.Parse(firstMatch.Groups[2].Value);

            ep.RangeStart = int.Parse(firstMatch.Groups[1].Value);
            ep.RangeEnd = leftEp;

            return ep;
        }

        public static Result<BatchEpisode> ValidateAndParse(HtmlNode input)
        {
            return ParserWithValidation<HtmlNode, BatchEpisode>.ParseWithValidation(
                input,
                Validate,
                Parse
            );
        }
    }
}
