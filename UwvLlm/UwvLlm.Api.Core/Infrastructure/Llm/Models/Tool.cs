namespace UwvLlm.Core.Infrastructure.Llm.Models;

public record Tool(
    string Name,
    string Desciption,
    ToolParameter[] Parameters);
