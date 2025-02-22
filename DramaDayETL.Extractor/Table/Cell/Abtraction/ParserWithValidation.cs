using DramaDayETL.Core.Abtraction;

namespace DramaDayETL.Extractor.Table.Cell.Abtraction
{
    public static class ParserWithValidation<TInput, TResult>
    {
        public static Result<TResult> ParseWithValidation(
            TInput input,
            Func<TInput, Result> validator,
            Func<TInput, Result<TResult>> parser)
        {
            var validationResult = validator(input);
            if (!validationResult.IsSuccess)
            {
                return Result.Failure<TResult>(validationResult.Error);
            }

            return parser(input);
        }
    }
}
