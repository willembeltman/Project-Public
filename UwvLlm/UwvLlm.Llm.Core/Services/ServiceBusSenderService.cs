using System.Text.Json;
using UwvLlm.Shared.Private.Dtos;
using UwvLlm.Shared.Private.Services;

namespace UwvLlm.Llm.Core.Services;

public class ServiceBusSenderService : BaseServiceBusSenderService
{
    public async Task SendGenerateAutoReplyResponse(GenerateAutoReplyResponse payload, CancellationToken ct)
    {
        var message = new ServiceBusMessage(
            MessageType: nameof(GenerateAutoReplyResponse),
            Payload: JsonSerializer.Serialize(payload));
        await SendAsync(message, ct);
    }
}
