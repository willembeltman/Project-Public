namespace UwvLlm.App.Interfaces;

public interface IMailService
{
    Task Send(Guid? to, string? subject, string? body);
}