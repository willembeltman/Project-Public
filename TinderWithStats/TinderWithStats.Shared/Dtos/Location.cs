using gAPI.Core.Attributes;
using gAPI.Core.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace TinderWithStats.Shared.Dtos;

[IsAuthorized]
public class Location : ICrudEntity
{
    [Key]
    public int Id { get; set; }
    [Required]
    [IsName]
    public string LocationName { get; set; } = string.Empty;
    [IsReadOnly]
    public bool CanUpdate { get; set; }
    [IsReadOnly]
    public bool CanDelete { get; set; }
    public override string ToString() => $"{LocationName}";
}