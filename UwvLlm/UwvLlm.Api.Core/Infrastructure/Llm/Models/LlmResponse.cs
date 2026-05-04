namespace UwvLlm.Core.Infrastructure.Llm.Models;

public record LlmResponse(
    string model,
    DateTime created_at,
    Message Message);
