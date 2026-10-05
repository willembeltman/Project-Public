using gAPI.Core.Attributes;
using System.ComponentModel.DataAnnotations;

namespace TinderWithStats.Backend.Entities;

[IsAuthorized]
public class Match
{
    [Key]
    public Guid Id { get; set; }

    public virtual Profile? ProfileSender { get; set; }
    public Guid ProfileSenderId { get; set; }

    public virtual Profile? ProfileReceiver { get; set; }
    public Guid ProfileReceiverId { get; set; }

    [IsName(gAPI.Core.Enums.FormattingOption.yyyy_MM_dd)]
    public DateTimeOffset MatchCreated { get; set; } = DateTimeOffset.Now;
    [IsName(gAPI.Core.Enums.FormattingOption.yyyy_MM_dd)]
    public DateTimeOffset? MatchAccepted { get; set; }
    public DateTimeOffset? MatchRemoved { get; set; }

    public virtual ICollection<ChatMessage>? ChatMessages { get; set; }
}