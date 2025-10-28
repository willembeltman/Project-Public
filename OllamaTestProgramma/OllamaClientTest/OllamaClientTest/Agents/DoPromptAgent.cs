using OllamaAgentGenerator.Services;
using System.IO;

namespace OllamaAgentGenerator.Agents;

public class DoPromptAgent(
    string userPromptText,
    FileRepositoryService fileRepository)
{
    public string GeneratePrompt(string? compileErrors = null)
    {
        fileRepository.InitializeFileTracking();

        string fileContentsText = fileRepository.FileContents.Count == 0
            ? "<No files in directory>"
            : string.Join(Environment.NewLine, fileRepository.FileContents);

        return @$"
The current source contents are:
{fileContentsText}

{(string.IsNullOrWhiteSpace(compileErrors) ? string.Empty : @$"
The current compile errors are:
{compileErrors}
")}

You are connected to a local Machine Control Protocol (MCP) that accepts file operations using the following exact commands:

%CreateOrUpdateFile(""path"", ""content"")%
  - Creates or replaces the file at the given ""path"".
  - The ""content"" parameter must escape newline characters as \n.

%MoveFile(""oldPath"", ""newPath"")%
  - Moves or renames a file from ""oldPath"" to ""newPath"".

%DeleteFile(""path"")%
  - Deletes the file at the given ""path"".

Formatting rules:
- Always start a command with % and end it with "")% . Pay extra attention to the ending "")% .
- Do not include any additional text, explanations, or Markdown outside of a %... )% block.
- Commands must match the examples exactly, including quotation marks and commas.
- Never explain what you are doing — just output the command itself.
- The ""content"" parameter must use DOUBLE escaping:
      \\n for newline
      \\r for carriage return
      \\\\ for backslash
      \\\"" for double quotes

Example:
%CreateOrUpdateFile(""scripts\test.txt"", ""Hello\nWorld!"")%

When you decide to modify the files, respond only with the appropriate MCP command.
Do not wrap your response in code blocks, Markdown, or any extra text.

User prompt:
{userPromptText}
";
    }


    public bool ProcessResponse(string responseText)
        => fileRepository.ProcessResponse(responseText);
}