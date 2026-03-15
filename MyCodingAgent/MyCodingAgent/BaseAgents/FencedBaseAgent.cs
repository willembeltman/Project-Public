//using MyCodingAgent.Models;
//using MyCodingAgent.Ollama;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Text.Json;
//using System.Text.RegularExpressions;

//namespace MyCodingAgent.BaseAgents;

//public class FencedBaseAgent(
//    Workspace workspace)
//{
//    protected JsonSerializerOptions JsonDeserializerOptions { get; } =
//        new() { PropertyNameCaseInsensitive = true };

//    protected Workspace workspace { get; } = workspace;

//    protected string GetActionsText()
//    {
//        return @"
//You interact with the workspace using fenced action blocks.

//AVAILABLE ACTIONS

//open_file(path: string)
//find(searchText: string)
//find_and_replace(path: string, searchText: string, replaceText: string)
//find_and_replace_all(searchText: string, replaceText: string)
//create_or_update_file(path: string, content: string)
//partial_overwrite_file(path: string, startLine: number, endLine: number, content: string)
//move_file(path: string, newPath: string)
//delete_file(path: string)
//create_or_update_task(path: string, content: string)
//delete_task(path: string)
//ask_developer_extra_information(content: string)

//EXAMPLE RESPONSE

//# action
//```find
//searchText: searchText is always one line
//```

//# action
//```find_and_replace
//path: Program.cs
//searchText: Using multiple lines for find_and_replace doesn't work
//replaceText: Using multiple lines for find_and_replace is too fragile
//```

//# action
//```partial_overwrite_file
//path: Program.cs
//startLine: 8
//endLine: 9
//content:
//    Console.WriteLine(""This is the first line of code with 4 leading spaces"");
//    Console.WriteLine(""This is the second line of code with 4 leading spaces"");
//```

//IMPORTANT RULES

//1. Always lead with # action
//2. Do NOT include explanations outside the actions.
//3. Only use the actions listed above.
//4. Never assume file contents, open the file first.
//5. Target .NET 10.
//6. If you need to modify a file you MUST first open_file.
//7. You may return multiple actions in a single response.
//8. Only use the parameters defined for each action.
//9. Every action MUST be inside a fenced block, of the actionname type.
//10. Parameter 'path', 'newPath', 'searchText' and 'replaceText' cannot have multiple lines
//11. Parameter 'content' is last and can have multiple lines
//";
//    }
//    protected string GetReducedActionsText()
//    {
//        return @"
//You interact with this system through a tool-DSL with fenced blocks command protocol.

//AVAILABLE ACTIONS
//find_and_replace(path: string, searchText: string, replaceText: string)
//find_and_replace_all(searchText: string, replaceText: string)
//open_file(path: string)
//create_or_update_file(path: string, content: string)
//partial_overwrite_file(path: string, startLine: number, endLine: number, content: string)
//move_file(path: string, newPath: string)
//delete_file(path: string)
//ask_developer_extra_information(content: string)

//EXAMPLE RESPONSE

//# action
//```find_and_replace
//path: Program.cs
//searchText: Using multiple lines for replace doesn't work
//replaceText: Using multiple lines for replace is too fragile
//```

//# action
//```partial_overwrite_file
//path: Program.cs
//startLine: 8
//endLine: 9
//content:
//    Console.WriteLine(""This is 1 line of code with 4 leading spaces"");
//```

//# action
//```ask_developer_extra_information
//content:
//What do you mean with 'Make a game'? What kind of game do you want me to make?
//```

//IMPORTANT RULES
//1. Always lead with # action
//2. Do NOT include explanations outside the actions.
//3. Only use the actions listed above.
//4. Never assume file contents, open the file first.
//5. Target .NET 10.
//6. If you need to modify a file you MUST first open_file.
//7. You may return multiple actions in a single response.
//8. Only use the parameters defined for each action.
//9. Every action MUST be inside a fenced block.
//10. Parameter 'path', 'newPath', 'searchText' and 'replaceText' cannot contain new lines
//11. Parameter 'content' always starts on the next line and can be multiple lines

//";
//    }

//    public async Task<bool> ProcessResponse(OllamaPrompt prompt, AgentResponse agentResponse)
//    {
//        var previousColor = Console.ForegroundColor;

//        (var agentActionCollection, var parseError) =
//            TryParseActions(agentResponse.responseText);

//        if (agentActionCollection == null || parseError != null)
//        {
//            workspace.AgentResponseResults.Add(
//                new AgentResponseResult(prompt, agentResponse, parseError, []));

//            Console.ForegroundColor = ConsoleColor.Red;
//            Console.WriteLine(parseError);

//            Console.ForegroundColor = previousColor;

//            return false;
//        }

//        var changed = false;
//        var list = new List<AgentActionResult>();

//        foreach (var action in agentActionCollection.actions)
//        {
//            var result = (string?)null;

//            switch (action.type)
//            {
//                case "find":
//                    result = await workspace.Find(action.searchText!);
//                    break;

//                case "find_and_replace":
//                    result = await workspace.FindAndReplace(
//                        action.path!,
//                        action.searchText!,
//                        action.replaceText!);
//                    changed = true;
//                    break;

//                case "find_and_replace_all":
//                    result = await workspace.FindAndReplaceAll(
//                        action.searchText!,
//                        action.replaceText!);
//                    changed = true;
//                    break;

//                case "open_file":
//                    result = await workspace.OpenFile(action.path!);
//                    break;

//                case "create_or_update_file":
//                    result = await workspace.CreateOrUpdateFile(
//                        action.path!,
//                        action.content!);
//                    changed = true;
//                    break;

//                case "delete_file":
//                    result = await workspace.DeleteFile(action.path!);
//                    changed = true;
//                    break;

//                case "move_file":
//                    result = await workspace.MoveFile(
//                        action.path!,
//                        action.newPath!);
//                    break;

//                case "partial_overwrite_file":
//                    result = await workspace.PartialOverwriteFile(
//                        action.path!,
//                        action.startLine!.Value,
//                        action.endLine!.Value,
//                        action.content!);
//                    changed = true;
//                    break;

//                case "create_or_update_task":
//                    result = await workspace.CreateOrUpdateTask(
//                        action.path!,
//                        action.content!);
//                    break;

//                case "delete_task":
//                    result = await workspace.DeleteTask(action.path!);
//                    break;

//                case "ask_developer_extra_information":
//                    result = await workspace.AskDeveloper(action.content!);
//                    break;

//                default:
//                    result = $"Action '{action.type}' not found";
//                    changed = false;

//                    Console.ForegroundColor = ConsoleColor.Red;
//                    Console.WriteLine(result);

//                    Console.ForegroundColor = previousColor;

//                    break;
//            }

//            if (result != null)
//                list.Add(new AgentActionResult(action, result));
//        }

//        workspace.AgentResponseResults.Add(
//            new AgentResponseResult(prompt, agentResponse, null, [.. list]));

//        return changed;
//    }
//    private (AgentActionCollection? response, string? parseError) TryParseActions(string input)
//    {
//        var lines = input.Split('\n');
//        var actions = new List<AgentAction>();
//        var currentAction = (AgentAction?)null;
//        var readingContent = false;

//        for (var i = 0; i < lines.Length; i++)
//        {
//            var line = lines[i];

//            if (line.StartsWith("# action"))
//            {
//                // Reset
//                currentAction = null;
//                readingContent = false;
//            }
//            else if (currentAction == null)
//            {
//                if (line.StartsWith("```"))
//                {
//                    currentAction = new AgentAction()
//                    {
//                        type = line.Substring("```".Length).Trim(' '),
//                    };
//                    actions.Add(currentAction);
//                }
//            }
//            else
//            {
//                if (line.StartsWith("```"))
//                {
//                    currentAction = null;
//                    readingContent = false;
//                }
//                else
//                {
//                    if (line.StartsWith("type:")) currentAction.type = line.Substring("type:".Length).Trim(' ');
//                    else if (line.StartsWith("path:")) currentAction.path = line.Substring("path:".Length).Trim(' ');
//                    else if (line.StartsWith("newPath:")) currentAction.newPath = line.Substring("newPath:".Length).Trim(' ');
//                    else if (line.StartsWith("searchText:")) currentAction.searchText = line.Substring("searchText:".Length).Trim(' ');
//                    else if (line.StartsWith("replaceText:")) currentAction.replaceText = line.Substring("replaceText:".Length).Trim(' ');
//                    else if (line.StartsWith("startLine:")) currentAction.startLine = Convert.ToInt32(line.Substring("startLine:".Length).Trim(' '));
//                    else if (line.StartsWith("endLine:")) currentAction.endLine = Convert.ToInt32(line.Substring("endLine:".Length).Trim(' '));
//                    else if (line.StartsWith("content:"))
//                    {
//                        if (line.Trim(' ').Length > "content:".Length)
//                            currentAction.content = line.Substring("content:".Length).Trim(' ');
//                        readingContent = true;
//                    }
//                    else if (readingContent)
//                    {
//                        if (currentAction.content != null)
//                            currentAction.content += "\n";
//                        else
//                            currentAction.content += string.Empty;
//                        currentAction.content += line;
//                    }
//                    else
//                    {
//                        return new(null, $"Error reading parsing actions, unexpected characters, line {i + 1}: {line}");
//                    }
//                }
//            }
//        }

//        return new(new AgentActionCollection() { actions = actions }, null);
//    }
//}