namespace UwvLlm.Shared.Interfaces;

public interface IEmailService
{
    void Receive(string from, string[] to, string subject, string body);
}
