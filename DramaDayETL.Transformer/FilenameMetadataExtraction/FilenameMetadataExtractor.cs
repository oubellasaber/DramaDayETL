using System.Text.RegularExpressions;

namespace DramaDayETL.Transformer.FilenameMetadataExtraction
{
    public class FilenameMetadataExtractor
    {
        public static FilenameMetadata Extract(string filename)
        {
            var (season, episode) = ExtractSeasonAndEpisode(filename);

            return new FilenameMetadata
            {
                OriginalFilename = filename,
                Quality = GetQuality(filename),
                ReleaseGroup = GetReleaseGroup(filename),
                Network = GetNetwork(filename),
                RipType = GetRipType(filename),
                Season = season,
                Episode = episode
            };
        }

        public static string? GetQuality(string filename)
        {
            string[] qualities = { "720p", "1080p", "540p", "4K", "480p", "2160p", "1440p" };
            foreach (var quality in qualities)
            {
                if (filename.Contains(quality, StringComparison.OrdinalIgnoreCase))
                {
                    return quality;
                }
            }
            return null;
        }

        public static string? GetReleaseGroup(string filename)
        {
            var pattern = @"-(?<releasegroup>[a-zA-Z0-9]+)[.\s[]*\[?";
            var matches = Regex.Matches(filename, pattern);

            // Get the last match if it exists
            if (matches.Count > 0)
            {
                var lastMatch = matches[matches.Count - 1];
                return lastMatch.Groups["releasegroup"].Value;
            }

            return null;
        }

        public static string? GetNetwork(string filename)
        {
            // Dictionary with abbreviations or alternate names as keys, full names as values
            var networkMappings = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "friday", "friDay" },
            { "viki", "Viki" },
            { "viu", "Viu" },
            { "iq", "iQIYI" },
            { "nf", "Netflix" },
            { "dsnp", "Disney+" },
            { "tving", "TVING" },
            { "hs", "Hulu" },
            { "kcw", "KCW" },
            { "hulu", "Hulu" },
            { "amzn", "Amazon Prime Video" },
            { "wavve", "Wavve" },
            { "sbs", "SBS" },
            { "iqiyi", "iQIYI" },
            { "tvn", "tvN" },
            { "kbs", "KBS" },
            { "jtbc", "JTBC" },
            { "LINETV", "LINE TV"},
            { "kocawa", "Kocowa" },
            { "naver", "Naver" },
            { "cpng", "Coupang Play" },
            { "coupang+play", "Coupang Play" },
            { "watcha", "Watcha" },
            { "wetv", "WeTV" },
            { "wv", "Wavve" },
            { "wavve", "Wavve" },
            { "pmtp", "Paramount+" },
            { "hbo", "HBO" },
            { "paramount", "Paramount+" }
        };

            foreach (var entry in networkMappings)
            {
                if (filename.Contains(entry.Key, StringComparison.OrdinalIgnoreCase))
                {
                    return entry.Value; // Return the mapped full name
                }
            }

            return null;
        }

        public static string? GetRipType(string filename)
        {
            var ripType = Array.Exists(new[] { "next", "f1rst", "wanna" }, group => string.Equals(GetReleaseGroup(filename), group, StringComparison.OrdinalIgnoreCase)) ?
                "HDTV" :
                GetRipType(filename);

            if (ripType is not null)
            {
                return ripType;
            }

            // Dictionary mapping rip type variations to normalized names
            var ripTypeMappings = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "webrip", "WEBRip" },
                { "web-dl", "WEB-DL" },
                { "webdl", "WEB-DL" },
                { "bluray", "BluRay" },
                { "hdrip", "HDRip" },
                { "dvdrip", "DVDRip" },
                { "hdtv", "HDTV" },
                { "hd-tv", "HDTV" }
            };

            foreach (var entry in ripTypeMappings)
            {
                if (filename.Contains(entry.Key, StringComparison.OrdinalIgnoreCase))
                {
                    return entry.Value;
                }
            }

            return null;
        }

        public static (int? season, int? episode) ExtractSeasonAndEpisode(string filename)
        {
            string pattern = @"[Ss](?:eason)?\s*(\d{1,2})[Ee](\d{1,2})";
            Regex regex = new Regex(pattern);
            Match match = regex.Match(filename);

            if (match.Success)
            {
                int.TryParse(match.Groups[1].Value, out int season);
                int.TryParse(match.Groups[2].Value, out int episode);
                return (season, episode);
            }

            return (null, null);
        }
    }
}
