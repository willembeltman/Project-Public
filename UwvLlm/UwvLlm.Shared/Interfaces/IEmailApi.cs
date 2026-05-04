using gAPI.Attributes;
using UwvLlm.Shared.Dtos;

namespace UwvLlm.Shared.Interfaces;

[GenerateApi]
public interface IEmailApi
{
    Task Receive(EmailDto email);
}
