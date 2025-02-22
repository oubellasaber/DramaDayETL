using DramaDayETL.Core.Abtraction;
using DramaDayETL.Core.Abtraction.Pipeline;
using DramaDayETL.Extractor.Extentions;
using DramaDayETL.Extractor.Table.Cell;
using DramaDayETL.Extractor.Table.Cell.MediaVersions;
using DramaDayETL.Extractor.Table.Cell.Seasons;
using HtmlAgilityPack;

namespace DramaDayETL.Extractor.Table
{
    public class TableParser : IParser<HtmlNode, Result<IEnumerable<Season>>>
    {
        public static Result<IEnumerable<Season>> Parse(HtmlNode input)
        {
            List<HtmlNode> nodes = new();
            nodes.Add(input.OwnerDocument.DocumentNode.SelectSingleNode(@"//meta[@property = ""og:url""]"));
            nodes.AddRange(input.SelectNodes(".//tr"));
            RemoveUncesseryRowsFromTable(nodes);

            TablePipelineState state = new TablePipelineState();

            nodes.ForEach(node =>
            {
                Pipeline<TablePipelineState>
                .For(node, state)
                .Try(
                    parser: SeasonParsingHandler.Parse,
                    onSuccess: (season, state) => state.Seasons.Add(season),
                    isContinueOnSuccess: false
                )
                .Try(
                    parser: MediaVersionParsingHandler.Parse,
                    onSuccess: (version, state) => state.CurrentSeason?.MediaVersions.Add(version),
                    isContinueOnSuccess: true
                )
                .EnsureExists(
                    condition: state => state.CurrentSeason != null && !state.CurrentSeason.MediaVersions.Any(),
                    action: state => state.CurrentSeason?.MediaVersions.Add(new MediaVersion() { MediaVersionName = "default" })
                )
                .Try(
                    parser: EpisodeWithVersionsParsingHandler.Parse,
                    onSuccess: (episode, state) => state.CurrentMediaVersion?.Episodes.Add(episode)
                )
                .Finally(
                    // set up proper logging later
                    finalAction: ctx => Console.WriteLine($"Log {ctx.Node.InnerHtml}")
                 );
            });

            return state.Seasons;
        }

        private static void RemoveUncesseryRowsFromTable(List<HtmlNode> rows)
        {
            if (rows[1].IsHeader())
            {
                rows.RemoveAt(1);
            }

            rows.RemoveAll(r => r.IsEmptyRow() || r.IsPasswordRow());
        }
    }
}
