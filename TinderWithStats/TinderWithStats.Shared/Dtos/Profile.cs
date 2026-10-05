using gAPI.Core.Attributes;
using gAPI.Core.Enums;
using gAPI.Core.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace TinderWithStats.Shared.Dtos;

[IsAuthorized]
public class Profile : ICrudEntity
{
    [Key]
    public Guid Id { get; set; }
    [IsForeignName(nameof(UserId))]
    public string? UserName { get; set; }
    [IsForeignKey(typeof(User))]
    public Guid UserId { get; set; }
    [IsForeignName(nameof(LocationId))]
    public string? LocationName { get; set; }
    [IsForeignKey(typeof(Location))]
    public int LocationId { get; set; }
    [Required]
    [IsName]
    public string Name { get; set; } = string.Empty;
    [IsName(" (", FormattingOption.yyyy_MM_dd, ")")]
    public DateTime DateOfBirth { get; set; } = DateTime.Now;
    public int Length { get; set; }
    [Required]
    public string Beroep { get; set; } = string.Empty;
    [Required]
    public string Bio { get; set; } = string.Empty;
    [IsReadOnly]
    public bool CanUpdate { get; set; }
    [IsReadOnly]
    public bool CanDelete { get; set; }
    public override string ToString() => $"{Name} {DateOfBirth:yyyy-MM-dd}";
}