using System.ComponentModel.DataAnnotations;

namespace UwvLlm.Core.Infrastructure.Data;

public class UserNotificationQuickOption
{
    [Key]
    public long Id { get; set; }

    public long UserNotificationId { get; set; }
    public virtual UserNotification? UserNotification { get; set; }

    [MaxLength(16)]
    public string Value { get; set; } = string.Empty;
}