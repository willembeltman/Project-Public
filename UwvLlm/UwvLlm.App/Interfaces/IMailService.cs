namespace UwvLlm.App.Interfaces;

public interface IMailService
{
    Task Send(string? from, string? to, string? subject, string? body);
}