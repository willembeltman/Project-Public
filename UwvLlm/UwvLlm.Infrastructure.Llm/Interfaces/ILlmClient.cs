using UwvLlm.Api.Core.Infrastructure.Llm.Models;

namespace UwvLlm.Api.Core.Infrastructure.Llm.Interfaces;

public interface ILlmClient : IDisposable
{
    bool Initialized { get; }

    Task<Model[]> GetModels(CancellationToken ct = default);
    Task InitializeModelAsync(Model model, CancellationToken ct = default);
    Task<LlmResponse> ChatAsync(Model model, LlmRequest apiCall, CancellationToken ct = default);

    string CreateMessagesJson(Message[] messages);
    string CreateRequestJson(Model model, LlmRequest apiCall);
    string CreateToolsJson(Tool[] tools);
}