namespace MyCodingAgent.Agents;

using MyCodingAgent.AgentRequest;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

public class CodingAgent(
    Workspace workspace)
{
    public string GeneratePrompt(string compiler_output)
    {
        AgentWorkspaceFileBrowser? fileBrowser = null;

        if (workspace.FileBrowserPath != null &&
            workspace.Files.TryGetValue(workspace.FileBrowserPath, out var workspaceFile))
        {
            fileBrowser = new AgentWorkspaceFileBrowser(
                workspaceFile.RelativePath,
                workspaceFile.GetLines());
        }

        if (fileBrowser == null)
        {
            var file = workspace.Files.Values.FirstOrDefault();
            if (file != null)
            {
                fileBrowser = new AgentWorkspaceFileBrowser(
                    file.RelativePath,
                    file.GetLines());
            }
        }

        var searchResults = new List<AgentWorkspaceSearchResult>();

        if (workspace.SearchText != null)
        {
            foreach (var file in workspace.Files.Values)
            {
                var lines = file.GetLines();

                foreach (var line in lines)
                {
                    if (line.content.Contains(workspace.SearchText))
                    {
                        searchResults.Add(
                            new AgentWorkspaceSearchResult(
                                file.RelativePath,
                                line.lineNumber,
                                line.content));
                    }
                }
            }
        }

        var options = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
        var sb = new StringBuilder();

        if (workspace.AgentResponseResults.Count > 0)
        {
            sb.AppendLine("================================");
            sb.AppendLine("YOUR LAST RESPONSE");
            sb.AppendLine("================================");
            sb.AppendLine();

            foreach (var h in workspace.AgentResponseResults.OrderByDescending(a => a.Response.date).Take(1))
            {
                sb.AppendLine($"You at {h.Response.date}:");
                sb.AppendLine($"Thinking: {h.Response.thinkingText}");
                sb.AppendLine($"Responsed: {h.Response.responseText}");

                if (h.ParseError != null)
                {
                    sb.AppendLine($"Parse error: ERROR!!! {h.ParseError}. Please respond in json!");
                }
                else
                {
                    sb.AppendLine("Parse state: Succesfully parsed response");
                }

                if (h.Actions != null && h.Actions.Any())
                {
                    sb.AppendLine($"Parsed actions:");
                    foreach (var action in h.Actions)
                    {
                        sb.AppendLine($"Action: {JsonSerializer.Serialize(action.AgentAction, options)}");
                        sb.AppendLine($"Result: {action.Result}");
                        sb.AppendLine();
                    }
                }
                else
                {
                    sb.AppendLine($"Parsed actions: ERROR!!! No actions found in response, please respond in json!");
                    sb.AppendLine();
                }
            }
        }

        sb.AppendLine("================================");
        sb.AppendLine("USER REQUEST");
        sb.AppendLine("================================");
        sb.AppendLine();
        sb.AppendLine(workspace.UserPrompt);
        sb.AppendLine();

        sb.AppendLine("================================");
        sb.AppendLine("WORKSPACE");
        sb.AppendLine("================================");
        sb.AppendLine();

        if (fileBrowser != null)
        {
            sb.AppendLine("--------------------------------");
            sb.AppendLine("CURRENT OPENED FILE");
            sb.AppendLine("--------------------------------");
            sb.AppendLine();
            sb.AppendLine(fileBrowser.path);
            sb.AppendLine();
            foreach (var line in fileBrowser.lines)
            {
                sb.AppendLine($"{line.lineNumber, 4}|{line.content}");
            }
            sb.AppendLine();
        }

        sb.AppendLine("--------------------------------");
        sb.AppendLine("ALL PROJECT FILES");
        sb.AppendLine("--------------------------------");
        sb.AppendLine();

        if (workspace.Files.Count > 0)
            foreach (var file in workspace.Files.Values)
                sb.AppendLine($"{file.RelativePath} ({file.GetLineCount()} lines)");
        else
            sb.AppendLine("No files found in project");

        sb.AppendLine();

        sb.AppendLine("--------------------------------");
        sb.AppendLine("COMPILER OUTPUT");
        sb.AppendLine("--------------------------------");
        sb.AppendLine();

        if (!string.IsNullOrWhiteSpace(compiler_output))
            sb.AppendLine(compiler_output);
        else
            sb.AppendLine("No compiler errors.");

        sb.AppendLine();

        if (workspace.SearchText != null)
        {
            sb.AppendLine("--------------------------------");
            sb.AppendLine("SEARCH");
            sb.AppendLine("--------------------------------");
            sb.AppendLine();
            sb.AppendLine($"Query: {workspace.SearchText}");
            sb.AppendLine($"Results: ");

            foreach (var r in searchResults)
            {
                sb.AppendLine($"{r.path,4}:{r.lineNumber} {r.line}");
            }

            sb.AppendLine();
        }

        if (workspace.Tasks.Count > 0)
        {
            sb.AppendLine("--------------------------------");
            sb.AppendLine("TASKS");
            sb.AppendLine("--------------------------------");
            sb.AppendLine();

            foreach (var t in workspace.Tasks)
            {
                sb.AppendLine($"{t.Key}. {t.Value}");
            }

            sb.AppendLine();
        }

        if (workspace.Notes.Count > 0)
        {
            sb.AppendLine("--------------------------------");
            sb.AppendLine("NOTES");
            sb.AppendLine("--------------------------------");
            sb.AppendLine();

            foreach (var n in workspace.Notes)
            {
                sb.AppendLine($"{n.Key}. {n.Value}");
            }

            sb.AppendLine();
        }

        var workspaceText = sb.ToString();

        return $@"You are an autonomous software engineering agent operating inside a .NET 10 development workspace.

You interact with this system through a JSON command protocol.

================================
IMPORTANT RULES
================================

1. Your response MUST be valid JSON.
2. The JSON MUST contain an array called ""actions"".
3. Do NOT include explanations outside the JSON.
4. Only use the actions listed below.
5. If you modify an existing file you MUST open it first.
6. Never assume file contents.
7. Target .NET 10

================================
DEVELOPMENT LOOP
================================

1. Inspect workspace
2. Open files if needed
3. Apply minimal edits
4. Fix compiler errors

================================
AVAILABLE ACTIONS
================================

--------------------------------
Search for text
--------------------------------
{{
  ""type"": ""search"",
  ""content"": ""class Program""
}}

--------------------------------
Open file
--------------------------------
{{
  ""type"": ""open_file"",
  ""path"": ""Program.cs""
}}

--------------------------------
Create or overwrite file
--------------------------------
{{
  ""type"": ""create_or_update_file"",
  ""path"": ""MyClass.cs"",
  ""content"": ""file content here""
}}

--------------------------------
Overwrite specific lines
--------------------------------
{{
  ""type"": ""partial_overwrite_file"",
  ""path"": ""Program.cs"",
  ""startLine"": 10,
  ""endLine"": 20,
  ""content"": ""replacement lines""
}}

--------------------------------
Delete file
--------------------------------
{{
  ""type"": ""delete_file"",
  ""path"": ""OldFile.cs""
}}

--------------------------------
Move file
--------------------------------
{{
  ""type"": ""move_file"",
  ""path"": ""Old.cs"",
  ""newPath"": ""New.cs""
}}

--------------------------------
Create directory
--------------------------------
{{
  ""type"": ""create_directory"",
  ""path"": ""services""
}}

--------------------------------
Delete directory
--------------------------------
{{
  ""type"": ""delete_directory"",
  ""path"": ""old""
}}

--------------------------------
Create note
--------------------------------
{{
  ""type"": ""create_or_update_note"",
  ""id"": 1,
  ""content"": ""important info""
}}

--------------------------------
Delete note
--------------------------------
{{
  ""type"": ""delete_note"",
  ""id"": 1
}}

--------------------------------
Create task
--------------------------------
{{
  ""type"": ""create_or_update_task"",
  ""id"": 1,
  ""content"": ""implement feature""
}}

--------------------------------
Delete task
--------------------------------
{{
  ""type"": ""delete_task"",
  ""id"": 1
}}

--------------------------------
RESPONSE FORMAT
--------------------------------
{{
  ""actions"": []
}}

{workspaceText}";
    }

    //    public string GeneratePrompt(string compiler_output)
    //    {
    //        var fileBrowser = (AgentWorkspaceFileBrowser?)null;
    //        if (workspace.FileBrowserPath != null)
    //        {
    //            if (workspace.Files.TryGetValue(workspace.FileBrowserPath, out var workspaceFile))
    //            {
    //                fileBrowser = new AgentWorkspaceFileBrowser(
    //                    workspaceFile.RelativePath,
    //                    workspaceFile.FileContent);
    //            }
    //        }

    //        if (fileBrowser == null)
    //        {
    //            var file = workspace.Files.Values.FirstOrDefault();
    //            if (file != null)
    //            {
    //                fileBrowser = new AgentWorkspaceFileBrowser(
    //                    file.RelativePath,
    //                    file.FileContent);
    //            }
    //        }

    //        var search_text = workspace.SearchText;
    //        var search_results = new List<AgentWorkspaceSearchResult>();
    //        if (workspace.SearchText != null)
    //        {
    //            foreach (var file in workspace.Files.Values)
    //            {
    //                var lines = file.GetLines();
    //                foreach (var line in lines)
    //                {
    //                    if (line.content.Contains(workspace.SearchText))
    //                    {
    //                        search_results.Add(new AgentWorkspaceSearchResult(file.RelativePath, line.lineNumber, line.content));
    //                    }
    //                }
    //            }
    //        }

    //        var all_files = workspace.Files.Values.Select(a => new AgentWorkspaceFile(a.RelativePath, a.GetLineCount()));
    //        var tasks = workspace.Tasks.Select(a => new AgentWorkspaceTask(a.Key, a.Value));
    //        var notes = workspace.Notes.Select(a => new AgentWorkspaceNote(a.Key, a.Value));
    //        var history = workspace.AgentResponseResults.Take(3).Select(a => new AgentWorkspaceHistory(a.Response, a.ParseError, a.Actions));

    //        var request = new AgentWorkspace(
    //            fileBrowser,
    //            all_files.ToArray(),
    //            compiler_output,
    //            search_text,
    //            search_results.ToArray(),
    //            tasks.ToArray(),
    //            notes.ToArray(),
    //            history.ToArray());

    //        var requestJson = JsonSerializer.Serialize(request, new JsonSerializerOptions
    //        {
    //            WriteIndented = true
    //        });

    //        return $@"You are an autonomous software engineering agent operating inside a .NET development workspace.

    //You interact with this system through a JSON command protocol. The system behaves like a minimal Visual Studio console environment.
    //Your goal is to inspect the workspace, modify files, and resolve compile errors in order to fulfill the user's request.

    //If the workspace already contains code written by another developer. Do NOT assume files are empty or broken.
    //You should inspect files before modifying them.

    //IMPORTANT RULES

    //1. Your response MUST be valid JSON.
    //2. The JSON MUST contain an array called ""actions"".
    //3. Do NOT include explanations outside the JSON.
    //4. Only use the actions listed below.
    //5. Prefer minimal edits when possible (use partial_overwrite_file).
    //6. Work iteratively: inspect → edit → compile → fix errors.
    //7. If a file already exists you MUST open it before modifying it.
    //8. Never overwrite a file you have not inspected.
    //9. Only create new files if they do not already exist.
    //10. Only create directories if they are required.

    //HOW TO WORK

    //Follow this development loop:

    //1. Inspect the workspace state.
    //2. If a file is mentioned in an error message, open it first.
    //3. If you need to modify an existing file, open it first using ""open_file"".
    //4. After inspecting files, apply minimal changes.
    //5. Prefer ""partial_overwrite_file"" instead of rewriting entire files.
    //6. Only use ""create_or_update_file"" when creating a completely new file or when the file is clearly unusable.

    //FILES

    //The workspace JSON lists all available files.  
    //If a file appears in this list it already exists.

    //To read the contents of a file you MUST use:

    //{{
    //  ""type"": ""open_file"",
    //  ""path"": ""SnakeGame.cs""
    //}}

    //Do not guess file contents.

    //COMMAND PROTOCOL

    //Each action has the following structure:

    //{{
    //  ""type"": ""action_name"",
    //  ""parameters"": ...
    //}}

    //AVAILABLE ACTIONS

    //Search for a string inside all files (results will be available through workspace JSON search_results):

    //{{
    //  ""type"": ""search"",
    //  ""content"": ""class Program""
    //}}

    //Open a file from the workspace (opened file available through workspace JSON file_browser):

    //{{
    //  ""type"": ""open_file"",
    //  ""path"": ""Program.cs""
    //}}

    //Create or overwrite a file:

    //{{
    //  ""type"": ""create_or_update_file"",
    //  ""path"": ""MyClass.cs"",
    //  ""content"": ""full file content here""
    //}}

    //Delete a file:

    //{{
    //  ""type"": ""delete_file"",
    //  ""path"": ""OldFile.cs""
    //}}

    //Move or rename a file:

    //{{
    //  ""type"": ""move_file"",
    //  ""path"": ""OldName.cs"",
    //  ""newPath"": ""NewName.cs""
    //}}

    //Create a directory:

    //{{
    //  ""type"": ""create_directory"",
    //  ""path"": ""services""
    //}}

    //Delete a directory:

    //{{
    //  ""type"": ""delete_directory"",
    //  ""path"": ""old""
    //}}

    //Overwrite specific lines in a file:

    //{{
    //  ""type"": ""partial_overwrite_file"",
    //  ""path"": ""Program.cs"",
    //  ""startLine"": 10,
    //  ""endLine"": 20,
    //  ""content"": ""replacement lines""
    //}}

    //Store persistent notes (for your future self):

    //{{
    //  ""type"": ""create_or_update_note"",
    //  ""id"": 1,
    //  ""content"": ""important information about the project""
    //}}

    //Delete stored notes:

    //{{
    //  ""type"": ""delete_note"",
    //  ""id"": 1
    //}}

    //Create or update a task:

    //{{
    //  ""type"": ""create_or_update_task"",
    //  ""id"": 1,
    //  ""content"": ""Implement user authentication""
    //}}

    //Delete a task:

    //{{
    //  ""type"": ""delete_task"",
    //  ""id"": 1
    //}}

    //RESPONSE FORMAT

    //You must ALWAYS respond using this JSON structure:

    //{{
    //  ""actions"": []
    //}}

    //USER REQUEST

    //{workspace.UserPrompt}

    //WORKSPACE STATE

    //The following JSON describes the current workspace.

    //{requestJson}
    //";
    //    }

    public async Task<bool> ProcessResponse(AgentResponse agentResponse)
    {
        if (!TryParseActions(agentResponse.responseText, out var agentActionCollection, out var parseError)) 
        {
            workspace.AgentResponseResults.Add(new AgentResponseResult(agentResponse, parseError, []));
            return false;
        }

        var found = false;
        var list = new List<AgentActionResult>();
        foreach (var agentAction in agentActionCollection.Actions)
        {
            found = true;
            var result = (string?)null;
            switch (agentAction.type)
            {
                case "search":
                    result = await SearchAsync(agentAction.content!);
                    break;

                case "open_file":
                    result = await OpenFileAsync(agentAction.path!);
                    break;

                case "create_or_update_file":
                    result = await CreateOrUpdateFileAsync(agentAction.path!, agentAction.content!);
                    break;

                case "delete_file":
                    result = await DeleteFileAsync(agentAction.path!);
                    break;

                case "move_file":
                    result = await MoveFileAsync(agentAction.path!, agentAction.newPath!);
                    break;

                case "create_directory":
                    result = await CreateDirectoryAsync(agentAction.path!);
                    break;

                case "delete_directory":
                    result = await RemoveDirectoryAsync(agentAction.path!);
                    break;

                case "partial_overwrite_file":
                    result = await PartialOverwriteFileAsync(
                        agentAction.path!,
                        agentAction.startLine!.Value,
                        agentAction.endLine!.Value,
                        agentAction.content!
                    );
                    break;

                case "create_or_update_note":
                    result = await CreateOrUpdateNoteAsync(agentAction.id!, agentAction.content!);
                    break;

                case "delete_note":
                    result = await DeleteNoteAsync(agentAction.id!);
                    break;

                case "create_or_update_task":
                    result = await CreateOrUpdateTaskAsync(agentAction.id!, agentAction.content!);
                    break;

                case "delete_task":
                    result = await DeleteTaskAsync(agentAction.id!);
                    break;

                default:
                    result = $"Action '{agentAction.type}' not found";
                    found = false;
                    break;
            }
            list.Add(new AgentActionResult(agentAction, result));
        }
        workspace.AgentResponseResults.Add(new AgentResponseResult(agentResponse, null, [.. list]));
        await workspace.Save();
        return found;
    }

    private bool TryParseActions(string responseText, out AgentActionCollection response, out string parseError)
    {
        response = new AgentActionCollection();
        parseError = string.Empty;
        var json = Clean(responseText);
        try
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var newResponse = JsonSerializer.Deserialize<AgentActionCollection>(json, options);
            if (newResponse == null) return false;
            response = newResponse;
            return true;
        }
        catch (Exception ex)
        {
            parseError = ex.Message;
            return false;
        }
    }
    string Clean(string input)
    {
        input = input.Trim();

        if (input.StartsWith("```"))
        {
            var firstNewline = input.IndexOf('\n');
            var lastFence = input.LastIndexOf("```");

            if (firstNewline >= 0 && lastFence > firstNewline)
            {
                input = input.Substring(firstNewline + 1, lastFence - firstNewline - 1);
            }
        }

        return input.Trim();
    }

    // MCP INTERFACE
    private async Task<string> SearchAsync(string searchText)
    {
        workspace.SearchText = searchText;
        return $"Changed search text to: '{searchText}'";
    }

    private async Task<string> OpenFileAsync(string path)
    {
        if (workspace.Files.TryGetValue(path, out _))
        {
            workspace.FileBrowserPath = path;
            return $"Opened workspace file '{path}'";
        }
        else
        {
            return $"Tried to open workspace file '{path}': Error, file not found";
        }
    }
    private async Task<string> CreateDirectoryAsync(string path)
    {
        workspace.TryParseFullPath(path, out var fullPath);
        try
        {
            Directory.CreateDirectory(fullPath);
            return $"Created directory '{path}'";
        }
        catch (Exception ex)
        {
            return $"Tried to created directory '{path}': Error {ex}";
        }
    }
    private async Task<string> RemoveDirectoryAsync(string path)
    {
        workspace.TryParseFullPath(path, out var fullPath);
        try
        {
            if (Directory.Exists(fullPath))
            {
                Directory.Delete(fullPath, true);
                return $"Removed directory '{path}'";
            }
            else
            {
                return $"Tried to remove directory '{path}': Error directory does not exist";
            }
        }
        catch (Exception ex)
        {
            return $"Tried to remove directory '{path}': Error {ex}";
        }
    }
    private async Task<string> CreateOrUpdateFileAsync(string path, string fullContent)
    {
        workspace.TryParseFullPath(path, out var fullPath);
        var fileInfo = new FileInfo(fullPath);
        if (fileInfo.Directory == null)
            throw new Exception($"Weird stuff, directory is empty? {fileInfo}");
        if (fileInfo.Directory.Exists == false)
            fileInfo.Directory.Create();

        await File.WriteAllTextAsync(fullPath, fullContent);

        workspace.Files[path] =
            new WorkspaceFile(path, fullPath, fullContent);

        return $"Updated {path}";
    }
    private async Task<string> PartialOverwriteFileAsync(string path, int startLineNr, int endLineNr, string newContent)
    {
        try
        {
            workspace.TryParseFullPath(path, out var fullPath);
            var file = workspace.Files[path];
            var lines = file.FileContent.Split('\n').ToList();
            var newLines = newContent.Split('\n');

            lines.RemoveRange(startLineNr - 1, endLineNr - startLineNr + 1);
            lines.InsertRange(startLineNr - 1, newLines);

            var content = string.Join("\n", lines);

            await File.WriteAllTextAsync(fullPath, content);

            workspace.Files[path] =
                new WorkspaceFile(path, fullPath, content);

            return $"Updated '{path}'";
        }
        catch (Exception ex)
        {
            return $"Tried to update '{path}', startLine {startLineNr}, endLineNr {endLineNr}: Error {ex.Message}";
        }
    }
    private async Task<string> MoveFileAsync(string path, string newPath)
    {
        workspace.TryParseFullPath(path, out var fullPath);
        workspace.TryParseFullPath(newPath, out var newFullPath);

        if (File.Exists(fullPath))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(newFullPath)!);

            File.Move(fullPath, newFullPath, true);
            workspace.Files.Remove(path);

            var content = await File.ReadAllTextAsync(newFullPath);
            workspace.Files[newPath] =
                new WorkspaceFile(path, fullPath, content);

            return $"Moved {path} -> {newPath}";
        }
        return $"Error: could not find '{path}'";
    }
    private async Task<string> DeleteFileAsync(string path)
    {
        workspace.TryParseFullPath(path, out var fullPath);

        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
            workspace.Files.Remove(path);

            return $"Deleted '{path}'";
        }
        return $"Error: could not find '{path}'";
    }
    private async Task<string> CreateOrUpdateNoteAsync(string id, string content)
    {
        workspace.Notes[id] = content;
        return $"Updated note '{id}'";
    }
    private async Task<string> DeleteNoteAsync(string id)
    {
        workspace.Notes.Remove(id);
        return $"Removed note '{id}'";
    }
    private async Task<string> CreateOrUpdateTaskAsync(string id, string content)
    {
        workspace.Tasks[id] = content;
        return $"Updated task '{id}'";
    }
    private async Task<string> DeleteTaskAsync(string id)
    {
        workspace.Tasks.Remove(id);
        return $"Removed task '{id}'";
    }
}
