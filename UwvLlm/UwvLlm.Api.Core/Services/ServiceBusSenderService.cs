using System.Text.Json;
using UwvLlm.Shared.Private.Dtos;
using UwvLlm.Shared.Private.Services;

namespace UwvLlm.Api.Core.Services;

public class ServiceBusSenderService : BaseServiceBusSenderService
{
    public async Task SendGenerateAutoReplyRequest(GenerateAutoReplyRequest payload, CancellationToken ct)
    {
        var message = new ServiceBusMessage(
            MessageType: nameof(GenerateAutoReplyRequest),
            Payload: JsonSerializer.Serialize(payload));
        await SendAsync(message, ct);
    }
}
