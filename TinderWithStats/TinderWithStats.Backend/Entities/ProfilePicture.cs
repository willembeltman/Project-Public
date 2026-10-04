using gAPI.Core.Attributes;
using gAPI.Core.Server.Storage;
using System.ComponentModel.DataAnnotations;

namespace TinderWithStats.Backend.Entities;

[IsAuthorized]
public class ProfilePicture : IStorageFile
{
    [Key]
    public Guid Id { get; set; }

    public virtual Profile? Profile { get; set; }
    public Guid ProfileId { get; set; }

    public DateTimeOffset UploadDate { get; set; } = DateTimeOffset.Now;

    string IStorageFile.Id => Id.ToString();
}