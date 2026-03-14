using MyCodingAgent.Compile;
using MyCodingAgent.Models;
using MyCodingAgent.Helpers;
using System.Text;
using MyCodingAgent.Interfaces;
using MyCodingAgent.BaseAgents;

public class DebuggingAgent(Workspace workspace) : FencedBaseAgent(workspace), IAgent
{
    public async Task<string> GeneratePrompt(CompileResult compileResult)
    {
        var sb = new StringBuilder();

        foreach (var fileGroup in compileResult.Errors.GroupBy(a => a.File))
        {
            var relativePath = fileGroup.Key;
            if (string.IsNullOrWhiteSpace(relativePath)) continue;

            var file = workspace.GetFile(relativePath);
            if (file != null)
            {
                sb.AppendLine(relativePath);
                sb.AppendLine("ERRORS");
                foreach (var error in fileGroup)
                {
                    sb.AppendLine(error.FullError.TrimEnd('\n').TrimEnd('\r'));
                }

                sb.AppendLine();
                sb.AppendLine("CODE");
                var fileContent = await file.GetFileContent();
                foreach (var line in fileContent.GetLines())
                {
                    sb.AppendLine($"{line.lineNumber,3}|{line.content}");
                }
                sb.AppendLine();
            }
        }

        var workspaceText = sb.ToString();
        var actionsText = GetActionsText();

        return $@"You are a .NET 10 compiler repair agent.

{actionsText}

{workspaceText}

GOAL
Make the code compile successfully.
Do not change behavior unless required.";
    }
}