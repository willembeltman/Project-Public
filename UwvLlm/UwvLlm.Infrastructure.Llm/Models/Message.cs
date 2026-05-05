using UwvLlm.Infrastructure.Llm.Enums;

namespace UwvLlm.Api.Core.Infrastructure.Llm.Models;

public record Message(
    Role Role,
    string? ToolCallId,
    string? Content,
    string? Thinking,
    ToolCall[]? ToolCalls);
