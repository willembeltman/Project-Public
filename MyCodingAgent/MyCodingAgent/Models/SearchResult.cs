namespace MyCodingAgent.Models;

public record SearchResult(
    string path,
    int lineNumber,
    string line);