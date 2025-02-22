using DramaDayETL.Core.Abtraction;
using DramaDayETL.Core.Abtraction.Pipeline;
using HtmlAgilityPack;

namespace DramaDayETL.Extractor.Table.Cell.MediaVersions
{
    internal class MediaVersionParsingHandler : IParser<HtmlNode, Result<MediaVersion>>
    {
        public static Result<MediaVersion> Parse(HtmlNode input)
        {
            ValueErrorState<MediaVersion> mediaVersionState = new();

            Pipeline<ValueErrorState<MediaVersion>>
                .For(input, mediaVersionState)
                .Try(
                    parser: HorizontalMediaVersionParser.ValidateAndParse,
                    onSuccess: (horizontalMediaVersion, state) => state.Value = horizontalMediaVersion,
                    onFailure: (result, state) => state.Error ??= result.Error
                )
                .Try(
                    parser: SidebarMediaVersionParser.ValidateAndParse,
                    onSuccess: (sidebarMediaVersion, state) => state.Value = sidebarMediaVersion,
                    onFailure: (result, state) => state.Error ??= result.Error
                );

            return ReferenceEquals(mediaVersionState.Value, null)
                ? Result.Failure<MediaVersion>(mediaVersionState.Error!)
                : Result.Success<MediaVersion>(mediaVersionState.Value);
        }
    }
}
