using gAPI.Attributes;
using UwvLlm.Shared.Dtos;

namespace UwvLlm.Shared.Interfaces;

[GenerateApi]
public interface IMailApi
{
    Task Receive(MailDto email);
}
