using DramaDayETL.Transformer.FileCryptExtractor.Entities.Rows.Enums;

namespace DramaDayETL.Transformer.FileCryptExtractor.Entities.Rows.ValueObjects;

public record Link(Uri Url, Status Status);