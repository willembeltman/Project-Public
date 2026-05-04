using System.ComponentModel.DataAnnotations;

namespace UwvLlm.Core.Infrastructure.Data;

public class MailMessageToUser
{
    [Key]
    public long Id { get; set; }

    public Guid MailMessageId { get; set; }
    public virtual MailMessage? MailMessage { get; set; }

    public Guid UserId { get; set; }
    public virtual User? User { get; set; }
}