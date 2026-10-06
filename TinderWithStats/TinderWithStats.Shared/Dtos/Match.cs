using gAPI.Core.Attributes;
using gAPI.Core.Enums;
using gAPI.Core.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace TinderWithStats.Shared.Dtos;

[IsAuthorized]
public class Match : ICrudEntity
{
    [Key]
    public Guid Id { get; set; }
    [IsForeignName(nameof(ProfileSenderId))]
    public string? ProfileSenderName { get; set; }
    [IsForeignKey(typeof(Profile))]
    public Guid ProfileSenderId { get; set; }
    [IsForeignName(nameof(ProfileReceiverId))]
    public string? ProfileReceiverName { get; set; }
    [IsForeignKey(typeof(Profile))]
    public Guid ProfileReceiverId { get; set; }
    [IsName(FormattingOption.yyyy_MM_dd)]
    public System.DateTimeOffset MatchCreated { get; set; }
    [IsName(FormattingOption.yyyy_MM_dd)]
    public System.DateTimeOffset? MatchAccepted { get; set; }
    public System.DateTimeOffset? MatchRemoved { get; set; }
    [IsReadOnly]
    public bool CanUpdate { get; set; }
    [IsReadOnly]
    public bool CanDelete { get; set; }
    public override string ToString() => $"{MatchCreated:yyyy-MM-dd} {MatchAccepted:yyyy-MM-dd}";
}