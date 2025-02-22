using DramaDayETL.Core.Abtraction;
using HtmlAgilityPack;
using System.Buffers.Text;
using System.Text;

namespace DramaDayETL.Extractor.MediaInfo
{
    internal class KoreanTitleParser : IParser<HtmlNode, Result<string>>,
        IValidator<HtmlNode, Result>
    {
        public static Result Validate(HtmlNode input)
        {
            HtmlNode titleNode = input.SelectSingleNode("//div[@class='wpb_wrapper']/p[contains(text(), 'Filename')]");

            if (titleNode is null)
                return Result.Failure(new Error("Media.KoreanTitle", "The html does not have the expected structure"));

            bool containsKrTitle = titleNode.InnerText.Any(c => IsKoreanChar(c));

            if (!containsKrTitle)
                return Result.Failure<string>(new Error("Media.KoreanTitle", "the title does not contain a korean word"));

            return Result.Success();
        }

        public static Result<string> Parse(HtmlNode input)
        {
            HtmlNode titleNode = input.SelectSingleNode("//div[@class='wpb_wrapper']/p[contains(text(), 'Filename')]");

            string titlesJoined = string.Join(":", titleNode.InnerText.Split(':').Skip(1));
            var titlesSeperated = titlesJoined.Split("/", StringSplitOptions.TrimEntries);
            var krTitle = GetKoreanTitle(titlesSeperated);

            return krTitle.Split("시즌")[0].Trim();
        }

        private static Predicate<char> IsKoreanChar = (c) => c >= '\u3131' && c <= '\u318E' || c >= '\uAC00' && c <= '\uD7A3';

        private static string GetKoreanTitle(IEnumerable<string> titles) =>
            titles.First(t => t.Any(c => IsKoreanChar(c)));
    }
}
