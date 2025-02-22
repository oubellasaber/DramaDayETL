using DramaDayETL.Core.Abtraction;
using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace DramaDayETL.Extractor.Table.Cell.Validators
{
    internal class LinkGroupsValidator : IValidator<HtmlNode, Result>
    {
        public static Result Validate(HtmlNode input)
        {
            var tdNodes = input.SelectNodes(".//td");
            bool isValid = false;

            if (tdNodes.Count == 2)
                isValid = true;

            if (tdNodes.Count == 3)
                isValid = Regex.IsMatch(tdNodes[2].InnerText, @"((?:[\w\s]+(?:\s*\|\s*)?)+)") && tdNodes[2].SelectNodes(".//a") != null;

            if (!isValid)
                return Result.Failure(Error.NotExpectedFormat);

            return Result.Success();
        }
    }
}
