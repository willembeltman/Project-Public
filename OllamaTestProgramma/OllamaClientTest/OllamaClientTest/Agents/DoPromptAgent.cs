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
  - The ""content"" parameter must use escaping:
      \n for newline
      \r for carriage return
      \\ for backslash
      \"" for double quotes

%MoveFile(""oldPath"", ""newPath"")%
  - Moves or renames a file from ""oldPath"" to ""newPath"".

%DeleteFile(""path"")%
  - Deletes the file at the given ""path"".

Formatting rules:
- Always start a command with % and end it with )%.
- Do not include any additional text, explanations, or Markdown outside of a %... )% block.
- Commands must match the examples exactly, including quotation marks and commas.
- The content must always be provided on a single line (no literal newlines).
- Never explain what you are doing — just output the command itself.

Example:
%CreateOrUpdateFile(""scripts/test.txt"", ""Console.WriteLine(\\\""Hello\\\\nWorld!\\\"");"")%

When you decide to modify files, respond only with the appropriate MCP command.
Do not wrap your response in code blocks, Markdown, or any extra text.

User prompt:
{userPromptText}
"
;
    }


    public bool ProcessResponse(string responseText)
        => fileRepository.ProcessResponse(responseText);
}