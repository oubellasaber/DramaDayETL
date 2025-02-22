using DramaDayETL.Core.Abtraction;
using DramaDayETL.Extractor.Table.Cell.Abtraction;
using DramaDayETL.Extractor.Table.Cell.Validators;
using HtmlAgilityPack;

namespace DramaDayETL.Extractor.Table.Cell.LinksGroup
{
    internal class NoTableLinksGroupParser : IParser<HtmlNode, Result<ICollection<ICollection<ShortLink>>>>,
        IValidator<HtmlNode, Result>
    {
        public static Result<ICollection<ICollection<ShortLink>>> Parse(HtmlNode input)
            => LinksGroupParsingUtility.ParseAsLinks(input.SelectNodes(".//td").ToList()).ToList();

        public static Result Validate(HtmlNode input)
        {
            var linkGroupsValidationResult = LinkGroupsValidator.Validate(input);
            if (linkGroupsValidationResult.IsFailure)
            {
                return linkGroupsValidationResult;
            }


            var nodes = input.SelectNodes(".//td");
            if (!nodes.All(n => n.OriginalName == "p"))
                return Result.Failure(Error.NotExpectedFormat);

            return Result.Success();
        }

        public static Result<ICollection<ICollection<ShortLink>>> ValidateAndParse(HtmlNode input)
        {
            return ParserWithValidation<HtmlNode, ICollection<ICollection<ShortLink>>>.ParseWithValidation(
                input,
                Validate,
                Parse
            );
        }
    }
}
