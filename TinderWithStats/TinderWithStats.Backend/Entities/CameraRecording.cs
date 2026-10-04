using gAPI.Core.Attributes;
using gAPI.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace Bsd.Infrastructure.Data.Entities;

[IsAuthorized]
public class CameraRecording
{
    [Key]
    public long Id { get; set; }

    public long CameraId { get; set; }
    public virtual Camera? Camera { get; set; }

    [IsName]
    public Guid ClientRecordingId { get; set; }
    [IsName(" ", FormattingOption.ToString)] public int FrameWidth { get; set; }
    [IsName("x", FormattingOption.ToString)] public int FrameHeight { get; set; }
    [IsName(" ", FormattingOption.ToString, "fps")] public double Fps { get; set; }
    public bool HasAudio { get; set; }


    [IsName(" (", gAPI.Core.Enums.FormattingOption.yyyy_MM_dd_HH_mm, ")")]
    public DateTimeOffset DateCreated { get; set; }
    public DateTimeOffset DateUpdated { get; set; }

    public virtual ICollection<CameraRecordingListener>? CameraRecordingListeners { get; set; }
} 