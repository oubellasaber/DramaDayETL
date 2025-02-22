using HtmlAgilityPack;

namespace DramaDayETL.Core.Abtraction.Pipeline
{
    public class Pipeline<TInput, TOutput>
    {
        private readonly PipelineContext<TInput, TOutput> _context;

        protected Pipeline(PipelineContext<TInput, TOutput> context)
        {
            _context = context;
        }

        public static Pipeline<TInput, TOutput> For(TInput node, TOutput output)
            => new(new PipelineContext<TInput, TOutput>(node, output));

        public Pipeline<TInput, TOutput> Try<T>(
            Func<TInput, Result<T>> parser,
            Action<T, TOutput> onSuccess,
            Action<Result<T>, TOutput>? onFailure = null,
            bool isContinueOnSuccess = false)
        {
            if (!_context.ContinueProcessing) return this;

            var result = parser(_context.Node);
            if (result.IsSuccess)
            {
                onSuccess(result.Value, _context.State);
                _context.ContinueProcessing = isContinueOnSuccess;
            }
            else
            {
                onFailure?.Invoke(result, _context.State);
            }
            return this;
        }

        public Pipeline<TInput, TOutput> Try<T>(
            Func<TInput, Result<T>> parser,
            bool isContinue = false) where T : TOutput
        {
            if (!_context.ContinueProcessing) return this;

            var result = parser(_context.Node);
            if (result.IsSuccess)
            {
                _context.State = result.Value;
                _context.ContinueProcessing = isContinue;
            }

            return this;
        }

        public Pipeline<TInput, TOutput> EnsureExists(
            Func<TOutput, bool> condition,
            Action<TOutput> action)
        {
            if (!_context.ContinueProcessing) return this;

            if (condition(_context.State))
            {
                action(_context.State);
            }
            return this;
        }

        public Pipeline<TInput, TOutput> TransformState(
            Action<TOutput> transformation)
        {
            if (!_context.ContinueProcessing) return this;

            transformation.Invoke(_context.State);

            return this;
        }

        public Result<TOutput> Finally(Action<PipelineContext<TInput, TOutput>> finalAction)
        {
            if (_context.ContinueProcessing)
            {
                finalAction(_context);
            }

            return GetOutput();
        }

        public Result<TOutput> GetOutput()
        {
            if (_context.State is null)
                return Result.Failure<TOutput>(Error.NoItemInPipelineMatched);

            return _context.State;
        }
    }

    public class Pipeline<TState> : Pipeline<HtmlNode, TState>
    {
        private Pipeline(PipelineContext<HtmlNode, TState> context) : base(context)
        {
        }

        public static new Pipeline<TState> For(HtmlNode node, TState initialState)
            => new(new PipelineContext<HtmlNode, TState>(node, initialState));
    }
}