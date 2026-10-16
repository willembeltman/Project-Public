using gAPI.Core.Attributes;
using gAPI.Core.Enums;
using gAPI.Core.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Rekenkamer.Shared.Dtos;

[IsAuthorized]
[HasLink("Toon Document", "DocumentDetails")]
public class Document : ICrudEntity
{
    [Key]
    public Guid Id { get; set; }
    [IsName]
    [Required(AllowEmptyStrings = false)]
    public string FullName { get; set; } = string.Empty;
    [IsName(" (", FormattingOption.yyyy_MM_dd, ")")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public long Size { get; set; }
    [Required]
    public string RelativePath { get; set; } = string.Empty;
    [Required]
    public string Name { get; set; } = string.Empty;
    [IsReadOnly]
    public bool CanUpdate { get; set; }
    [IsReadOnly]
    public bool CanDelete { get; set; }
    public override string ToString() => $"{FullName} {CreatedAt:yyyy-MM-dd}";
}