using gAPI.Core.Attributes;
using System.ComponentModel.DataAnnotations;

namespace TinderWithStats.Backend.Entities;

[IsHidden]
public class UserRole
{
    [Key]
    public Guid Id { get; set; }

    public virtual User? User { get; set; }
    public Guid UserId { get; set; }

    public virtual Role? Role { get; set; }
    public Guid RoleId { get; set; }
}