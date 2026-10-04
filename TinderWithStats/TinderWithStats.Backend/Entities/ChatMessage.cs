using gAPI.Core.Attributes;
using System.ComponentModel.DataAnnotations;

namespace TinderWithStats.Backend.Entities;

public class ChatMessage
{
    [Key]
    public Guid Id { get; set; }

    public virtual Match? Match { get; set; }
    public Guid MatchId { get; set; }

    public virtual Profile? ProfileSender { get; set; }
    public Guid ProfileSenderId { get; set; }

    public virtual Profile? ProfileReceiver { get; set; }
    public Guid ProfileReceiverId { get; set; }

    [IsName(gAPI.Core.Enums.FormattingOption.yyyy_MM_dd_HH_mm)]
    public DateTimeOffset Date { get; set; } = DateTimeOffset.Now;
    [IsName]
    public string Message { get; set; } = string.Empty;
}
