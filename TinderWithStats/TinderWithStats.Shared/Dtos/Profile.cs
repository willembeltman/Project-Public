using gAPI.Core.Attributes;
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
    [IsForeignKey(typeof(Location))]
    public int LocationId { get; set; }
    public System.DateTimeOffset DateOfBirth { get; set; }
    public int Length { get; set; }
    [Required]
    public string Beroep { get; set; } = string.Empty;
    [Required]
    public string Bio { get; set; } = string.Empty;
    [IsReadOnly]
    public bool CanUpdate { get; set; }
    [IsReadOnly]
    public bool CanDelete { get; set; }
}