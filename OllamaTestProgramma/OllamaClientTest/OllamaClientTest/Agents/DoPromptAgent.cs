using OllamaAgentGenerator.Services;

namespace OllamaAgentGenerator.Agents;

public class DoPromptAgent(
    string userPromptText,
    FileRepositoryService fileRepository)
{
    public string GeneratePrompt(string? compileErrors = null)
    {
        var fileContentsText = fileRepository.GenerateFileContentsText();
        var prompt = $"The current source contents is:\n{fileContentsText}\n\n";

        if (!string.IsNullOrWhiteSpace(compileErrors))
        {
            prompt += $"The current compile errors are:\n{compileErrors}\n\n";
        }

        prompt += $"The user prompt was:\n{userPromptText}\n\n";

        var mcpText = fileRepository.GenerateMcpCommandsText();
        prompt += $"Please choose from actions to solve the user prompt(you can do multiple):\n{mcpText}";
        return prompt; 
    }

    public void ProcessResponse(string responseText)
        => fileRepository.ProcessResponse(responseText);
}