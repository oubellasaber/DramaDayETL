using DramaDayETL.Core.Abtraction;

namespace DramaDayETL.Transformer.DownloadLinkExtraction
{
    public interface IDownloadLinkExtractor
    {
        bool CanHandleUrlAsync(Uri url);
        ValueTask<Result<Uri>> GetDownloadUrlAsync(Uri url);
    }
}