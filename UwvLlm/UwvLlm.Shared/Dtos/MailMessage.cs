using gAPI.Attributes;
using gAPI.Enums;
using gAPI.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace UwvLlm.Shared.Dtos;

public class MailMessage : ICrudEntity
{
    [Key]
    public Guid Id { get; set; }
    [Required]
    public string From { get; set; } = string.Empty;
    [Required]
    public string Subject { get; set; } = string.Empty;
    [Required]
    public string Body { get; set; } = string.Empty;
    [IsReadOnly]
    public bool CanUpdate { get; set; }
    [IsReadOnly]
    public bool CanDelete { get; set; }
}