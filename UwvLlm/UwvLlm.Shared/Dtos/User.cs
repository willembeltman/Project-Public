using gAPI.Attributes;
using gAPI.Interfaces;
using gAPI.Storage;
using System.ComponentModel.DataAnnotations;

namespace UwvLlm.Shared.Dtos;

[IsAuthorized]
[IsUser]
[IsEntryPoint]
public class User : ICrudEntity, IStorageFileDto
{
    [Key]
    public Guid Id { get; set; }
    [IsName]
    [StringLength(128, MinimumLength = 0)]
    [Required(AllowEmptyStrings = false)]
    public string UserName { get; set; } = string.Empty;
    [StringLength(255, MinimumLength = 0)]
    [Required(AllowEmptyStrings = false)]
    public string Email { get; set; } = string.Empty;
    [Required]
    [StringLength(32, MinimumLength = 0)]
    public string PhoneNumber { get; set; } = string.Empty;
    [IsReadOnly]
    [IsStorageFileUrlProperty]
    public string? StorageFileUrl { get; set; }
    [IsReadOnly]
    public bool CanUpdate { get; set; }
    [IsReadOnly]
    public bool CanDelete { get; set; }
    public override string ToString() => $"{UserName}";
}