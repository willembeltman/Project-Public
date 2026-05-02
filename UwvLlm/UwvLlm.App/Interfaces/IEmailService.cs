namespace UwvLlm.App.Interfaces;

public interface IEmailService
{
    Task Send(string? from, string? to, string? subject, string? body);
}