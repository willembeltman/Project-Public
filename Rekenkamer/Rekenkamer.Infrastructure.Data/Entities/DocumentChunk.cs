using gAPI.Core.Attributes;
using Pgvector;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rekenkamer.Infrastructure.Data.Entities;

[IsAuthorized]
public class DocumentChunk
{
    [Key]
    public Guid Id { get; set; }

    public Guid DocumentId { get; set; }

    [ForeignKey(nameof(DocumentId))]
    public Document Document { get; set; } = null!;

    [Required]
    [IsName]
    public string Content { get; set; } = string.Empty;

    public int PageNumber { get; set; }
    public int ChunkIndex { get; set; }
    public double Left { get; set; }
    public double Width { get; set; }
    public double Top { get; set; }
    public double Height { get; set; }
    public float? ParagraphConfidence { get; set; }

    [IsHidden]
    public Vector? Vector { get; set; }
}