using gAPI.Attributes;
using System.ComponentModel.DataAnnotations;

namespace UwvLlm.Core.Infrastructure.Data;

[IsHidden]
public class MailMessage
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid FromUserId { get; set; } = default!;
    public virtual User FromUser { get; set; } = default!;

    [IsName]
    public string Subject { get; set; } = string.Empty;
    [IsName("(", gAPI.Enums.FormattingOption.dd_MM_yyyy_HH_mm, ")")]
    public DateTimeOffset Date { get; set; }
    public string Body { get; set; } = string.Empty;

    public virtual ICollection<MailMessageToUser> ToUsers { get; set; } = [];
}