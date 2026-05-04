namespace UwvLlm.Api.Core.Infrastructure.Llm.Models;

public record ToolCall(
    string Id,
    ToolCallFunction Function);
