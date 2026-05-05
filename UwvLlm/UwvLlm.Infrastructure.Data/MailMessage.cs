using gAPI.Core.Attributes;
using gAPI.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace UwvLlm.Infrastructure.Data;

public class MailMessage
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid FromUserId { get; set; } = default!;
    public virtual User FromUser { get; set; } = default!;

    public Guid ToUserId { get; set; } = default!;
    public virtual User ToUser { get; set; } = default!;

    [IsName]
    public string Subject { get; set; } = string.Empty;

    [IsName(" (", FormattingOption.dd_MM_yyyy_HH_mm, ")")]
    public DateTimeOffset Date { get; set; }

    public string Body { get; set; } = string.Empty;
    public string? AutoResponse { get; set; }
}