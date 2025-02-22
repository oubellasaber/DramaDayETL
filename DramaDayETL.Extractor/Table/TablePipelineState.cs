using DramaDayETL.Core.Abtraction;
using DramaDayETL.Extractor.Table.Cell.Episodes.Entities;
using DramaDayETL.Extractor.Table.Cell.MediaVersions;
using DramaDayETL.Extractor.Table.Cell.Seasons;
using System.Collections.ObjectModel;

namespace DramaDayETL.Extractor.Table
{
    public class TablePipelineState
    {
        public Error? Error { get; set; }
        public Collection<Season> Seasons { get; } = new();
        public Season? CurrentSeason => Seasons.LastOrDefault();
        public MediaVersion? CurrentMediaVersion => CurrentSeason?.MediaVersions.LastOrDefault();
        public Episode? CurrentEpisode => CurrentMediaVersion?.Episodes.LastOrDefault();
    }
}