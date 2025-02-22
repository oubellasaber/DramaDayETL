using DramaDayETL.Core.Abtraction;
using DramaDayETL.Core.Abtraction.Pipeline;
using HtmlAgilityPack;

namespace DramaDayETL.Extractor.Table.Cell.LinksGroup
{
    internal class LinkGroupParsingHandler : IParser<HtmlNode, Result<ICollection<ICollection<ShortLink>>>>
    {
        public static Result<ICollection<ICollection<ShortLink>>> Parse(HtmlNode input)
        {
            ValueErrorState<ICollection<ICollection<ShortLink>>> linksGroupsState = new();

            Pipeline<ValueErrorState<ICollection<ICollection<ShortLink>>>>
                .For(input, linksGroupsState)
                .Try(
                    parser: NoTableLinksGroupParser.ValidateAndParse,
                    onSuccess: (noTableLinksGroups, state) => state.Value = noTableLinksGroups,
                    onFailure: (result, state) => state.Error ??= result.Error
                )
                .Try(
                    parser: ThreeCellLinksGroupParser.ValidateAndParse,
                    onSuccess: (threeCellLinksGroup, state) => state.Value = threeCellLinksGroup,
                    onFailure: (result, state) => state.Error ??= result.Error
                )
                .Try(
                    parser: TwoCellLinksGroupParser.ValidateAndParse,
                    onSuccess: (twoCellLinksGroup, state) => state.Value = twoCellLinksGroup,
                    onFailure: (result, state) => state.Error ??= result.Error
                );

            return ReferenceEquals(linksGroupsState.Value, null)
                ? Result.Failure<ICollection<ICollection<ShortLink>>>(linksGroupsState.Error!)
                : Result.Success(linksGroupsState.Value);
        }
    }
}
