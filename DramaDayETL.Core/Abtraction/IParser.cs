namespace DramaDayETL.Core.Abtraction
{
    public interface IParser<TInput, TResult>
    {
        public abstract static TResult Parse(TInput input);
    }
}
