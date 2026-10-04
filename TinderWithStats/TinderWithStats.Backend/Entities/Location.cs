using gAPI.Core.Attributes;
using System.ComponentModel.DataAnnotations;

namespace TinderWithStats.Backend.Entities;

[IsAuthorized]
public class Location
{
    [Key]
    public int Id { get; set; }

    public string LocationName { get; set; } = string.Empty;

    public virtual ICollection<Profile>? Profiles { get; set; }
}