namespace MyCodingAgent.Compile;

public class CompileResult
{
    public string Output { get; set; } = string.Empty;
    public List<CompileError> Errors { get; set; } = [];

    public override string ToString()
    {
        return Output;
    }
}