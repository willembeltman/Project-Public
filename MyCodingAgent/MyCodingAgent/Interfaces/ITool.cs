using MyCodingAgent.Models;
using MyCodingAgent.Ollama;

namespace MyCodingAgent.Interfaces;

public interface ITool
{
    string Name {  get; }
    string Desciption { get; }
    ToolParameter[] Parameters { get; }
    Task<ToolResult> Invoke(OllamaResponseMessageToolCallFunctionArguments toolArguments);
    public Tool ToDto()
    {
        return new Tool(Name, Desciption, Parameters);
    }
}