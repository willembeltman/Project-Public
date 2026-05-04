namespace UwvLlm.Api.Core.Infrastructure.Llm.Models;

public record Message(
    string Role,
    string? ToolCallId,
    string? Content,
    string? Thinking,
    ToolCall[]? ToolCalls);
