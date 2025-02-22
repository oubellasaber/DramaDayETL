using DramaDayETL.Core.Abtraction;
using DramaDayETL.Core.Abtraction.Pipeline;
using HtmlAgilityPack;

namespace DramaDayETL.Extractor.Table.Cell.Seasons
{
    internal class SeasonParsingHandler : IParser<HtmlNode, Result<Season>>
    {
        public static Result<Season> Parse(HtmlNode input)
        {
            ValueErrorState<Season> seasonState = new();

            Pipeline<ValueErrorState<Season>>
                .For(input, seasonState)
                .Try(
                    parser: HorizontalSeasonParser.ValidateAndParse,
                    onSuccess: (horizontalSeason, state) => state.Value = horizontalSeason,
                    onFailure: (result, state) => state.Error ??= result.Error
                )
                .Try(
                    parser: SidebarSeasonParser.ValidateAndParse,
                    onSuccess: (sidebarSeason, state) => state.Value = sidebarSeason,
                    onFailure: (result, state) => state.Error ??= result.Error
                )
                .Try(
                    parser: UrlSeasonParser.ValidateAndParse,
                    onSuccess: (urlSeason, state) => state.Value = urlSeason,
                    onFailure: (result, state) => state.Error ??= result.Error
                );

            return ReferenceEquals(seasonState.Value, null)
                ? Result.Failure<Season>(seasonState.Error!)
                : Result.Success<Season>(seasonState.Value);
        }
    }
}
