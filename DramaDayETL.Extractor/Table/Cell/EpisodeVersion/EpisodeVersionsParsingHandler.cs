using DramaDayETL.Core.Abtraction;
using DramaDayETL.Core.Abtraction.Pipeline;
using DramaDayETL.Extractor.Table.Cell.LinksGroup;
using DramaDayETL.Extractor.Table.Cell.QualitiesGroup;
using HtmlAgilityPack;

namespace DramaDayETL.Extractor.Table.Cell.EpisodeVersion
{
    internal class EpisodeVersionsParsingHandler : IParser<HtmlNode, Result<ICollection<EpVersion>>>
    {
        public static Result<ICollection<EpVersion>> Parse(HtmlNode input)
        {
            ValueErrorState<ICollection<ICollection<ShortLink>>> linkState = new();
            ValueErrorState<ICollection<string>> qualityState = new();

            Pipeline<ValueErrorState<ICollection<ICollection<ShortLink>>>>
                .For(input, linkState)
                .Try(
                    parser: LinkGroupParsingHandler.Parse,
                    onSuccess: (linkGroups, state) => state.Value = linkGroups,
                    onFailure: (result, state) => state.Error = result.Error
                );

            Pipeline<ValueErrorState<ICollection<string>>>
                .For(input, qualityState)
                .Try(
                    parser: QualityParsingHandler.Parse,
                    onSuccess: (qualityGroups, state) => state.Value = qualityGroups,
                    onFailure: (result, state) => state.Error = result.Error
                );

            if (ReferenceEquals(linkState.Value, null) || ReferenceEquals(qualityState.Value, null))
            {
                var error = linkState.Error ?? qualityState.Error;
                return Result.Failure<ICollection<EpVersion>>(error!);
            }

            var epVersions = linkState.Value
                .Zip(qualityState.Value, (links, versionName) => new EpVersion
                {
                    EpisodeVerisonName = versionName,
                    Links = links
                })
                .ToList();

            return Result.Success<ICollection<EpVersion>>(epVersions);
        }
    }
}
