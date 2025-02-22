using DramaDayETL.Core.Abtraction;
using HtmlAgilityPack;
using DramaDayETL.Core.Abtraction.Pipeline;
using DramaDayETL.Extractor.Table.Cell.Episodes;
using DramaDayETL.Extractor.Table.Cell.EpisodeVersion;
using DramaDayETL.Extractor.Table.Cell.Episodes.Entities;

namespace DramaDayETL.Extractor.Table.Cell
{
    internal class EpisodeWithVersionsParsingHandler : IParser<HtmlNode, Result<Episode>>
    {
        public static Result<Episode> Parse(HtmlNode input)
        {
            ValueErrorState<Episode> episodeState = new();

            Pipeline<ValueErrorState<Episode>>
                .For(input, episodeState)
                .Try(
                    parser: EpisodeParsingHandler.Parse,
                    onSuccess: (episode, state) => state.Value = episode,
                    onFailure: (result, state) => state.Error ??= result.Error,
                    isContinueOnSuccess: true
                )
                .Try(
                    parser: EpisodeVersionsParsingHandler.Parse,
                    onSuccess: (episodeVersions, state) => state.Value!.EpisodeVersions = episodeVersions,
                    onFailure: (result, state) => state.Error ??= result.Error
                );

            return ReferenceEquals(episodeState.Value, null)
                ? Result.Failure<Episode>(episodeState.Error!)
                : Result.Success<Episode>(episodeState.Value);
        }
    }
}
