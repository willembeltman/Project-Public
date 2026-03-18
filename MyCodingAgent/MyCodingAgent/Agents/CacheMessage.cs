namespace MyCodingAgent.Agents;

public record CacheMessage(
    string toolName,
    string? id,
    string? action, 
    string? path, 
    string? newPath,
    string? query,
    int? lineNumber);