using gAPI.Attributes;
using gAPI.Enums;
using gAPI.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace UwvLlm.Shared.Dtos;

public class MailMessage : ICrudEntity
{
    [Key]
    public Guid Id { get; set; }
    [IsForeignKey(typeof(User))]
    public Guid FromUserId { get; set; }
    [IsForeignName(nameof(FromUserId))]
    public string? FromUserName { get; set; }
    [IsForeignKey(typeof(User))]
    public Guid ToUserId { get; set; }
    [IsForeignName(nameof(ToUserId))]
    public string? ToUserName { get; set; }
    [Required]
    [IsName]
    public string Subject { get; set; } = string.Empty;
    [IsName(" (", FormattingOption.dd_MM_yyyy_HH_mm, ")")]
    public System.DateTimeOffset Date { get; set; }
    [Required]
    public string Body { get; set; } = string.Empty;
    [IsReadOnly]
    public bool CanUpdate { get; set; }
    [IsReadOnly]
    public bool CanDelete { get; set; }
    public override string ToString() => $"{Subject} {Date:dd-MM-yyyy HH:mm}";
}