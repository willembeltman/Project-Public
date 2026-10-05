using gAPI.Core.Attributes;
using gAPI.Core.Enums;
using gAPI.Core.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace TinderWithStats.Shared.Dtos;

[IsAuthorized]
[IsUser]
[IsEntryPoint]
public class User : ICrudEntity
{
    [Key]
    public Guid Id { get; set; }
    [IsName]
    [StringLength(128, MinimumLength = 0)]
    [Required(AllowEmptyStrings = false)]
    public string UserName { get; set; } = string.Empty;
    [IsName(" (", FormattingOption.ToString, ")")]
    [StringLength(255, MinimumLength = 0)]
    [Required(AllowEmptyStrings = false)]
    public string Email { get; set; } = string.Empty;
    [Required]
    [StringLength(32, MinimumLength = 0)]
    public string PhoneNumber { get; set; } = string.Empty;
    [IsReadOnly]
    public bool CanUpdate { get; set; }
    [IsReadOnly]
    public bool CanDelete { get; set; }
    public override string ToString() => $"{UserName} {Email}";
}