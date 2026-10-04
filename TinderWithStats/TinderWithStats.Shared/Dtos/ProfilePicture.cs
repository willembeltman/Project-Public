using gAPI.Core.Attributes;
using gAPI.Core.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace TinderWithStats.Shared.Dtos;

[IsAuthorized]
public class ProfilePicture : ICrudEntity, IStorageFileDto
{
    [Key]
    public Guid Id { get; set; }
    [IsForeignKey(typeof(Profile))]
    public Guid ProfileId { get; set; }
    public System.DateTimeOffset UploadDate { get; set; }
    [IsReadOnly]
    [IsStorageFileUrlProperty]
    public string? StorageFileUrl { get; set; }
    [IsReadOnly]
    public bool CanUpdate { get; set; }
    [IsReadOnly]
    public bool CanDelete { get; set; }
}