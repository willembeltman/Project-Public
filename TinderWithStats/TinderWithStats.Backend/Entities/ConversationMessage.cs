using Bsd.Infrastructure.Llm.Shared.Enums;
using gAPI.Core.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Bsd.Infrastructure.Data.Entities;


[IsAuthorized]
public class ConversationMessage
{
    [Key]
    public long Id { get; set; }

    public long ConversationId { get; set; }
    public virtual Conversation? Conversation { get; set; }

    public DateTimeOffset? Created { get; set; }
    public Role Role { get; set; }
    public string? Message { get; set; }
    public string? Thinking { get; set; }
}