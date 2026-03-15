using MyCodingAgent.BaseAgents;
using MyCodingAgent.Compile;
using MyCodingAgent.Helpers;
using MyCodingAgent.Models;
using MyCodingAgent.Ollama;
using System.Text;

namespace MyCodingAgent.Agents;

public class IsFinishedAgent(
    Workspace workspace)
    : YesNoBaseAgent
{
    public async Task<OllamaPrompt> GeneratePrompt(CompileResult compileResult)
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine("The current source contents is:");

        if (workspace.Files.Count == 0)
        {
            sb.AppendLine("<No files in directory>");
        }
        else
        {
            foreach (var a in workspace.Files)
            {
                var content = await a.GetFileContent();
                sb.AppendLine($"File '{a.RelativePath}'");
                foreach (var line in content.GetLines())
                {
                    sb.AppendLine($"{line.lineNumber,3}|{line.content}");
                }
            }
        }

        if (!string.IsNullOrWhiteSpace(compileResult.Output))
        {
            sb.AppendLine("The current compile result is:");
            sb.AppendLine(compileResult.Output);
        }
        var fullPrompt = $@"{sb}

The user prompt is:
{workspace.UserPrompt}

Is the prompt satisfied? Reply [YES] or [NO]

Response must include the [ and ] signs
";

        return new OllamaPrompt([new OllamaMessage(nameof(OllamaAgentRole.user), fullPrompt)], "[]");
    }
}