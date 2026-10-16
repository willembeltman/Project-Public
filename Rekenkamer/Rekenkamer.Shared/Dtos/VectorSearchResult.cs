namespace Rekenkamer.Shared.Dtos;


public class VectorSearchResult
{
    public Document Document { get; set; } = default!;
    public DocumentChunk MatchingDocumentChunk { get; set; } = default!;
    public DocumentChunk[] DocumentChunks { get; set; } = default!;
    public double Distance { get; set; }
}