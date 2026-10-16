using gAPI.Core.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Rekenkamer.Infrastructure.Data.Entities;

[IsAuthorized]
[HasLink("Toon Document", "DocumentDetails")]
public class Document
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [IsName]
    public string FullName { get; set; } = string.Empty;

    [IsName(" (", gAPI.Core.Enums.FormattingOption.yyyy_MM_dd, ")")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Relatie naar de chunks
    public List<DocumentChunk> Chunks { get; set; } = new();
    public long Size { get; set; }
    public string RelativePath { get; set; } = string.Empty;
    public string Name { get; set; }
}
