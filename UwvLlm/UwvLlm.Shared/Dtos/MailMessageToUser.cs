using gAPI.Attributes;
using gAPI.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace UwvLlm.Shared.Dtos;

[IsJunctionTable(typeof(MailMessage), typeof(User))]
public class MailMessageToUser : ICrudEntity
{
    [Key]
    public long Id { get; set; }
    [IsForeignKey(typeof(MailMessage))]
    public Guid MailMessageId { get; set; }
    [IsForeignName(nameof(MailMessageId))]
    public string? MailMessageName { get; set; }
    [IsForeignKey(typeof(User))]
    public Guid UserId { get; set; }
    [IsForeignName(nameof(UserId))]
    public string? UserName { get; set; }
    [IsReadOnly]
    public bool CanUpdate { get; set; }
    [IsReadOnly]
    public bool CanDelete { get; set; }
}