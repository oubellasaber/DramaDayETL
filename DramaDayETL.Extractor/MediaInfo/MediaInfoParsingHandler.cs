using DramaDayETL.Core.Abtraction;
using DramaDayETL.Extractor;
using HtmlAgilityPack;

namespace DramaDayETL.Extractor.MediaInfo
{
    internal class MediaInfoParsingHandler : IParser<HtmlNode, Result<Media>>
    {
        public static Result<Media> Parse(HtmlNode input)
        {
            var krTitle = KoreanTitleParser.Parse(input);
            var enTitle = EnglishTitleParser.Parse(input);

            if (enTitle.IsFailure)
                return Result.Failure<Media>(enTitle.Error);

            var id = DramaDayPostIdParser.Parse(input);

            if (id.IsFailure)
                return Result.Failure<Media>(id.Error);

            return new Media
            {
                DramaDayId = id.Value,
                KrTitle = krTitle.Value,
                EnTitle = enTitle.Value,
            };
        }
    }
}
