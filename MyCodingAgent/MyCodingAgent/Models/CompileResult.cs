namespace MyCodingAgent.Models;

public class CompileResult
{
    public string Output { get; set; } = string.Empty;
    public List<CompileError> Errors { get; set; } = [];
    public List<CompileError> Warnings { get; set; } = [];

    public override string ToString()
    {
        return Output;
    }
}