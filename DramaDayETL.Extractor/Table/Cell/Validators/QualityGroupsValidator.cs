using DramaDayETL.Core.Abtraction;
using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace DramaDayETL.Extractor.Table.Cell.Validators
{
    internal class QualityGroupsValidator : IValidator<HtmlNode, Result>
    {
        public static Result Validate(HtmlNode input)
        {
            var tdNodes = input.SelectNodes("./td");
            bool isValid = false;

            if (tdNodes.Count == 2)
            {
                var processedHtml = tdNodes[1].InnerHtml;

                isValid = Regex.IsMatch(tdNodes[1].InnerHtml,
                    @"(.+:)\s*((?:.*?\|\s*)*)",
                    RegexOptions.Singleline);
            }

            if (tdNodes.Count == 3)
            {
                isValid = Regex.IsMatch(tdNodes[1].InnerHtml, "^(?!.*<br>$).*$") &&
                    tdNodes[2].SelectSingleNode("./a") != null;
            }

            if (!isValid)
                return Result.Failure(Error.NotExpectedFormat);

            return Result.Success();
        }
    }
}
