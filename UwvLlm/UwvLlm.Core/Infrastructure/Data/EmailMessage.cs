using System.ComponentModel.DataAnnotations;

namespace UwvLlm.Core.Infrastructure.Data;

public class EmailMessage
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public string From { get; set; } = string.Empty;
    public string[] To { get; set; } = [];
    public string Subject { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
}