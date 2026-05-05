using gAPI.Core.Attributes;
using gAPI.Core.Interfaces;
using System.ComponentModel.DataAnnotations;
using UwvLlm.Shared.Enums;

namespace UwvLlm.Shared.Dtos;

public class UserNotification : ICrudEntity
{
    [Key]
    public long Id { get; set; }
    [IsForeignKey(typeof(User))]
    public Guid UserId { get; set; }
    [IsForeignName(nameof(UserId))]
    public string? UserName { get; set; }
    public NotificationType ExternalType { get; set; }
    [Required]
    public string ExternalId { get; set; } = string.Empty;
    [Required]
    public string Title { get; set; } = string.Empty;
    [Required]
    public string Message { get; set; } = string.Empty;
    [Required]
    public string[] QuickOptions { get; set; } = [];
    [IsReadOnly]
    public bool CanUpdate { get; set; }
    [IsReadOnly]
    public bool CanDelete { get; set; }
}