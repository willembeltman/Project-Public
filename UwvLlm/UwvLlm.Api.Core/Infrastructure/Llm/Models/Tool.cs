namespace UwvLlm.Api.Core.Infrastructure.Llm.Models;

public record Tool(
    string Name,
    string Desciption,
    ToolParameter[] Parameters);
