using DramaDayETL.Core.Abtraction;

namespace DramaDayETL.Core.Abtraction.Pipeline
{
    public class ValueErrorState<T>
    {
        public T? Value { get; set; }
        public Error? Error { get; set; }
    }
}