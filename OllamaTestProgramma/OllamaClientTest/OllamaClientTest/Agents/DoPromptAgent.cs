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

        var mcpText = fileRepository.GenerateMcpCommandsText();
        prompt += $"Please choose from these actions to solve the user prompt(you can do multiple):\n{mcpText}\n\n";

        prompt += $"The user prompt is:\n{userPromptText}";

        return prompt; 
    }

    public bool ProcessResponse(string responseText)
        => fileRepository.ProcessResponse(responseText);
}