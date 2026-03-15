namespace MyCodingAgent.Ollama;

public class OllamaResponseMessageToolCallFunctionArguments
{
    public string? path { get; set; }
    public string? newPath { get; set; }
    public string? searchText { get; set; }
    public string? replaceText { get; set; }
    public string? content { get; set; }
    public int? startLine { get; set; }
    public int? endLine { get; set; }
}
