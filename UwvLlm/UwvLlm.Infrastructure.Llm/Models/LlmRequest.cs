namespace UwvLlm.Api.Core.Infrastructure.Llm.Models;

public record LlmRequest(
    Message[] Messages,
    Tool[] Tools);
