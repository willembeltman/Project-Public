using gAPI.Core.Attributes;
using System.ComponentModel.DataAnnotations;

namespace TinderWithStats.Backend.Entities;

[IsAuthorized]
public class Profile
{
    [Key]
    public Guid Id { get; set; }

    public virtual User? User { get; set; }
    public Guid UserId { get; set; }

    public virtual Location? Location { get; set; }
    public int LocationId { get; set; }

    [IsName]
    public string Name { get; set; } = string.Empty;
    [IsName(" (", gAPI.Core.Enums.FormattingOption.yyyy_MM_dd, ")")]
    public DateTime DateOfBirth { get; set; }
    public int Length { get; set; }
    public string Beroep { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;

    public virtual ICollection<ProfilePicture>? ProfilePictures { get; set; }
    public virtual ICollection<Match>? MatchesSend { get; set; }
    public virtual ICollection<Match>? MatchesReceived { get; set; }
    public virtual ICollection<ChatMessage>? MessagesSend { get; set; }
    public virtual ICollection<ChatMessage>? MessagesReceived { get; set; }
}