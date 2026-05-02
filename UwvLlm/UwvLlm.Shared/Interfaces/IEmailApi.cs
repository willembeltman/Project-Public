namespace UwvLlm.Shared.Interfaces;

public interface IEmailApi
{
    void Receive(string from, string[] to, string subject, string body);
}
