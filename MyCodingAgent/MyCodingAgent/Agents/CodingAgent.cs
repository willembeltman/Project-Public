using MyCodingAgent.BaseAgents;
using MyCodingAgent.Compile;
using MyCodingAgent.Helpers;
using MyCodingAgent.Interfaces;
using MyCodingAgent.Models;
using MyCodingAgent.Ollama;
using System.Text;

namespace MyCodingAgent.Agents;

public class CodingAgent(Workspace workspace) : JsonBaseAgent(workspace), IAgent
{
    public async Task<OllamaPrompt> GeneratePrompt(CompileResult compileResult)
    {



        List<OllamaMessage> messageList = new List<OllamaMessage>();
        foreach (var a in workspace.AgentResponseResults)
        {
            messageList.Add(new OllamaMessage(nameof(OllamaAgentRole.user), a.request));

            var llmMessage = a.response.message.content;
            if (a.response.message.content == null && a.response.message.tool_calls.Length > 0)
            {
                llmMessage = string.Join("\n", a.response.message.tool_calls.Select(b => $@"tool_call: {b.function.name}"));
            }
            if (llmMessage != null)
            {
                messageList.Add(new OllamaMessage(nameof(OllamaAgentRole.assistant), llmMessage));
            }
        }

        var promptHelper = new PromptHelper(workspace);
        var sb = new StringBuilder();
        await promptHelper.LastResponse(sb);
        await promptHelper.ProjectFiles(sb);
        await promptHelper.CurrentOpenFile(sb);
        await promptHelper.SearchResults(sb);
        await promptHelper.CurrentPrompt(sb);
        await promptHelper.SavedTasks(sb);
        var workspaceText = $@"You are an autonomous software engineering agent operating inside a .NET 10 development workspace.
{sb}
WORKFLOW

1. Understand the request
2. Inspect files if needed
3. Make minimal edits
4. Verify using search or open_file

Always prefer small incremental steps.";

        messageList.Add(new OllamaMessage(nameof(OllamaAgentRole.user), workspaceText));


        string tools = CreateToolsString();

        return new OllamaPrompt(
            [.. messageList],
            tools);
    }
}