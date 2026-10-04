using gAPI.Core.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Bsd.Infrastructure.Data.Entities;

[IsAuthorized]
public class CameraRecordingListener
{
    [Key]
    public long Id { get; set; }

    public long CameraRecordingId { get; set; }
    public virtual CameraRecording? CameraRecording { get; set; }

    public Guid UserId { get; set; }
    public virtual User? User { get; set; }

    public string SessionId { get; set; } = string.Empty;
    public DateTimeOffset DateCreated { get; set; }
    public DateTimeOffset? DateClosed { get; set; }
    public string PongCode { get; set; }
} 