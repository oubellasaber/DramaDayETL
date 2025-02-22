using HtmlAgilityPack;

namespace DramaDayETL.Core.Abtraction.Pipeline
{
    public class PipelineContext<TInput, TState>
    {
        public TInput Node { get; }
        public TState State { get; set; }
        public bool ContinueProcessing { get; set; } = true;

        public PipelineContext(TInput node, TState state)
        {
            Node = node;
            State = state;
        }
    }

    public class PipelineContext<TState> : PipelineContext<HtmlNode, TState>
    {
        public PipelineContext(HtmlNode node, TState state) : base(node, state)
        {
        }
    }
}