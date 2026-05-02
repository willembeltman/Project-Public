using gAPI.Attributes;

namespace UwvLlm.Shared.Interfaces;

[GenerateHub]
public interface INotificationsHub
{
    Task ShowEmailReceived(string id, string text);
}
