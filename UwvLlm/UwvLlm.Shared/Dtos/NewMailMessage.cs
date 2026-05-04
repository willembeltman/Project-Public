namespace UwvLlm.Shared.Dtos;

public class NewMailMessage
{
    public string From { get; set; } = string.Empty;
    public string[] To { get; set; } = [];
    public string Subject { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
}