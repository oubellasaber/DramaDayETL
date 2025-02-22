using FFMpegCore;

namespace DramaDayETL.Transformer.VideoMetadataExtraction
{
    public class VideoMetadataExtractor
    {
        public static async Task<VideoMetadata> GeMetadata(Uri sourceStream)
        {
            var mediaInfo = await FFProbe.AnalyseAsync(sourceStream);

            var videoMetadata = new VideoMetadata
            (
                mediaInfo.Duration,
                mediaInfo.PrimaryVideoStream?.Height,
                mediaInfo.PrimaryVideoStream?.CodecLongName,
                mediaInfo.PrimaryVideoStream?.BitsPerRawSample,
                mediaInfo.PrimaryVideoStream?.AvgFrameRate,
                mediaInfo.PrimaryAudioStream?.CodecLongName,
                mediaInfo.PrimaryAudioStream?.Channels,
                mediaInfo.PrimaryAudioStream?.BitRate,
                mediaInfo.SubtitleStreams.Select(s => s.Language).ToList()
            );

            return videoMetadata;
        }
    }
}
