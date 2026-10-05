using gAPI.Core.Attributes;
using gAPI.Core.Enums;
using gAPI.Core.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace TinderWithStats.Shared.Dtos;

public class ChatMessage : ICrudEntity
{
    [Key]
    public Guid Id { get; set; }
    [IsForeignName(nameof(MatchId))]
    public string? MatchName { get; set; }
    [IsForeignKey(typeof(Match))]
    public Guid MatchId { get; set; }
    [IsForeignName(nameof(ProfileSenderId))]
    public string? ProfileSenderName { get; set; }
    [IsForeignKey(typeof(Profile))]
    public Guid ProfileSenderId { get; set; }
    [IsForeignName(nameof(ProfileReceiverId))]
    public string? ProfileReceiverName { get; set; }
    [IsForeignKey(typeof(Profile))]
    public Guid ProfileReceiverId { get; set; }
    [IsName(FormattingOption.yyyy_MM_dd_HH_mm)]
    public System.DateTimeOffset Date { get; set; }
    [Required]
    [IsName]
    public string Message { get; set; } = string.Empty;
    [IsReadOnly]
    public bool CanUpdate { get; set; }
    [IsReadOnly]
    public bool CanDelete { get; set; }
    public override string ToString() => $"{Date:yyyy-MM-dd HH:mm} {Message}";
}