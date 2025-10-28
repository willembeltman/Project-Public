using OllamaAgentGenerator.Services;
using System.IO;

namespace OllamaAgentGenerator.Agents;

public class DoPromptAgent(
    string userPromptText,
    FileRepositoryService fileRepository)
{
    public string GeneratePrompt(string? compileErrors = null)
    {
        var fileContentsText = string.Empty;
        fileRepository.InitializeFileTracking();
        if (fileRepository.FileContents.Count == 0) fileContentsText = "<No files in directory>";
        fileContentsText = string.Join(Environment.NewLine, fileRepository.FileContents);

        return @$"The current source contents is:
{fileContentsText}
{(!string.IsNullOrWhiteSpace(compileErrors) ? @$"
The current compile errors are:
{compileErrors}
" : "" )}
You are connected to a local Machine Control Protocol (MCP) that accepts file operations using the following commands:

%CreateOrUpdateFile(""path"", ""content"")%
Creates a new file or replaces the existing one at ""path"".
The ""content"" parameter must escape newlines as \n and carriage returns as \r.

%MoveFile(""oldPath"", ""newPath"")%
Moves or renames a file.

%DeleteFile(""path"")%
Deletes a file.

Formatting rules:

Always start a command with % and end it with )%.

Never include additional text, explanations, or Markdown outside of the %... )% block.

Commands must be written exactly as shown, including quotation marks and commas.

Example:

%CreateOrUpdateFile(""scripts/test.txt"", ""Hello\\nWorld!"")%

The user prompt is:
{userPromptText}
";
    }

    public bool ProcessResponse(string responseText)
        => fileRepository.ProcessResponse(responseText);
}