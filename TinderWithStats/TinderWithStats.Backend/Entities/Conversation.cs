using gAPI.Core.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Bsd.Infrastructure.Data.Entities;

[IsAuthorized]
[HasLink("Chat", "Chat")]
public class Conversation
{
    [Key]
    public long Id { get; set; }

    public Guid UserId { get; set; }
    public virtual User? User { get; set; }

    public DateTimeOffset? CreatedAt { get; set; }
    [IsName]
    public string? Title { get; set; }

    public virtual ICollection<ConversationMessage>? Messages { get; set; }
}