using DramaDayETL.Core.Abtraction;
using DramaDayETL.Core.Abtraction.Pipeline;
using DramaDayETL.Extractor.MediaInfo;
using DramaDayETL.Extractor.Table;
using HtmlAgilityPack;

namespace DramaDayETL.Extractor
{
    public class MediaParser : IParser<HtmlNode, Result<Media>>
    {
        public class MediaPipelineState
        {
            public Error? Error { get; set; }
            public Media Media { get; set; }
        }

        public static Result<Media> Parse(HtmlNode input)
        {
            MediaPipelineState state = new MediaPipelineState();

            Pipeline<MediaPipelineState>
               .For(input, state)
               .Try(
                   parser: MediaInfoParsingHandler.Parse,
                   onSuccess: (media, state) => state.Media = media,
                   onFailure: (result, state) => state.Error ??= result.Error,
                   isContinueOnSuccess: true
               )
               .Try(
                   parser: input => TableParser.Parse(input.SelectSingleNode(".//tbody")),
                   onSuccess: (seasons, state) => state.Media.Seasons = seasons,
                   onFailure: (result, state) => state.Error ??= result.Error,
                   isContinueOnSuccess: true
               );

            return state.Media;
        }
    }
}
