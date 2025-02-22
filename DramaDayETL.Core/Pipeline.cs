using DramaDayETL.Core.Abtraction;

namespace DramaDayETL.Core
{
    internal class PipelineWithState<TInput, TOutput, TState>
    {
        private readonly TInput _input;
        private readonly TState _state;
        private TOutput? _output;
        private bool _continueProcessing;

        private PipelineWithState(TInput input, TState state)
        {
            _input = input;
            _state = state;
            _continueProcessing = true;
        }

        public static PipelineWithState<TInput, TOutput, TState> For(TInput input, TState state)
        {
            return new(input, state);
        }

        public PipelineWithState<TInput, TOutput, TState> Try(
            Func<TInput, Result<TOutput>> parser,
            Action<TOutput, TState>? onSuccess = null,
            bool isContinue = false)
        {
            if (!_continueProcessing) return this;

            var result = parser(_input);
            if (result.IsSuccess)
            {
                _output = result.Value;
                onSuccess?.Invoke(result.Value, _state);
                _continueProcessing = isContinue;
            }

            return this;
        }

        public Result<TOutput> GetOutput()
        {
            if (_output is null)
                return Result.Failure<TOutput>(Error.NoItemInPipelineMatched);

            return Result.Success(_output);
        }

        public TState GetState() => _state;
    }
}
