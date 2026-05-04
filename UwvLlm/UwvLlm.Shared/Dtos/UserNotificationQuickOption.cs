using gAPI.Attributes;
using gAPI.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace UwvLlm.Shared.Dtos;

public class UserNotificationQuickOption : ICrudEntity
{
    [Key]
    public long Id { get; set; }
    [IsForeignKey(typeof(UserNotification))]
    public long UserNotificationId { get; set; }
    [Required]
    [MaxLength(16)]
    public string Value { get; set; } = string.Empty;
    [IsReadOnly]
    public bool CanUpdate { get; set; }
    [IsReadOnly]
    public bool CanDelete { get; set; }
}