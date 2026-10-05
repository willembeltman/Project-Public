using gAPI.Core.Attributes;
using System.ComponentModel.DataAnnotations;

namespace TinderWithStats.Backend.Entities;

[IsHidden]
public class Role
{
    [Key]
    public Guid Id { get; set; }

    [IsName]
    public string Name { get; set; } = string.Empty;
    
    public virtual ICollection<UserRole>? UserRoles { get; set; }
}