namespace DramaDayETL.Core.Abtraction
{
    public interface IValidator<TInput, TResult>
    {
        abstract static TResult Validate(TInput input);
    }
}
