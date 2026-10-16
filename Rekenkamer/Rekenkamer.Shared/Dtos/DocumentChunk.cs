using gAPI.Core.Attributes;
using gAPI.Core.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Rekenkamer.Shared.Dtos;

[IsAuthorized]
public class DocumentChunk : ICrudEntity
{
    [Key]
    public Guid Id { get; set; }
    [IsForeignKey(typeof(Document))]
    public Guid DocumentId { get; set; }
    [IsForeignName(nameof(DocumentId))]
    public string? DocumentName { get; set; }
    [IsName]
    [Required(AllowEmptyStrings = false)]
    public string Content { get; set; } = string.Empty;
    public int PageNumber { get; set; }
    public int ChunkIndex { get; set; }
    public double Left { get; set; }
    public double Width { get; set; }
    public double Top { get; set; }
    public double Height { get; set; }
    public System.Single? ParagraphConfidence { get; set; }
    [IsReadOnly]
    public bool CanUpdate { get; set; }
    [IsReadOnly]
    public bool CanDelete { get; set; }
    public override string ToString() => $"{Content}";
}