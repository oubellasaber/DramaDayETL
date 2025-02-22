namespace DramaDayETL.Transformer.VideoMetadataExtraction
{
    public class VideoMetadata
    {
        public TimeSpan Duration { get; set; }
        public int? Height { get; set; }
        public string? Encoding { get; set; }
        public int? ColorDepth { get; set; }
        public double? FrameRate { get; set; }
        public string? AudioCodec { get; set; }
        public int? AudioChannels { get; set; }
        public long? AudioBitrate { get; set; }
        public List<string?> InternalsSubtitles { get; set; }

        public VideoMetadata(
            TimeSpan duration,
            int? height,
            string? encoding,
            int? colorDepth,
            double? frameRate,
            string? audioCodec,
            int? audioChannels,
            long? audioBitrate,
            List<string?> internalsSubtitles)
        {
            Duration = duration;
            Height = height;
            Encoding = encoding;
            ColorDepth = colorDepth;
            FrameRate = frameRate;
            AudioCodec = audioCodec;
            AudioChannels = audioChannels;
            AudioBitrate = audioBitrate == 0 ? 256 : audioBitrate;
            InternalsSubtitles = internalsSubtitles;
        }

        public override string ToString()
        {
            return $@"
                    Duration:          {Duration}
                    Height:            {Height} pixels
                    Encoding:          {Encoding}
                    Color Depth:       {ColorDepth} bits
                    Frame Rate:        {FrameRate} fps
                    Audio Codec:       {AudioCodec}
                    Audio Channels:    {AudioChannels}
                    Audio Bitrate:     {AudioBitrate} kbps"
                    + (InternalsSubtitles.Count > 0 ? $"\n                    Subtitles:        {string.Join(", ", InternalsSubtitles)}" : "");
        }
    }
}