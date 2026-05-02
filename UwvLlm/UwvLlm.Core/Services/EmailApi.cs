using UwvLlm.Shared.Interfaces;

namespace UwvLlm.Core.Services;

public class EmailApi : IEmailApi
{
    public void Receive(string from, string[] to, string subject, string body)
    {
        throw new NotImplementedException();
    }
}
