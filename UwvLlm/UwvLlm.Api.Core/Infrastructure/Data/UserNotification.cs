using System.ComponentModel.DataAnnotations;
using UwvLlm.Shared.Enums;

namespace UwvLlm.Api.Core.Infrastructure.Data;

public class UserNotification
{
    [Key]
    public long Id { get; set; }

    public Guid UserId { get; set; }
    public virtual User? User { get; set; }

    public NotificationType ExternalType { get; set; }
    public string ExternalId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;

    public string[] QuickOptions { get; set; } = [];
}
