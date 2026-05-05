namespace UwvLlm.App.Core.Interfaces;

public interface IEmailService
{
    Task Send(Guid? toUserId, string? subject, string? body);
}