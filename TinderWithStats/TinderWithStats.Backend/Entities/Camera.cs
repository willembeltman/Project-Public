using gAPI.Core.Attributes;
using gAPI.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Bsd.Infrastructure.Data.Entities;

[IsAuthorized]
public class Camera
{
    [Key]
    public long Id { get; set; }

    [IsName] public string MachineName { get; set; } = default!;
    [IsName] public string Name { get; set; } = default!;
    public string CameraHardwareId { get; set; } = default!;
    public string Bus { get; set; } = default!;

    public virtual ICollection<CameraRecording>? CameraRecordings { get; set; }
}