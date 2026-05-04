namespace UwvLlm.Core.Infrastructure.Llm.Models;

public record ToolCall(
    string Id,
    ToolCallFunction Function);
