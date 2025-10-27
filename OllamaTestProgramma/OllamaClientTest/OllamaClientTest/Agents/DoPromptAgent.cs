using OllamaAgentGenerator.Services;

namespace OllamaAgentGenerator.Agents;

public class DoPromptAgent(
    string userPromptText,
    FileRepositoryService fileRepository)
{
    public bool WantsToSeeCompile => fileRepository.WantsToSeeCompile;

    public string GeneratePrompt(string? compileErrors = null)
    {
        var fileContentsText = fileRepository.GenerateFileContentsText();
        var mcpText = fileRepository.GenerateMcpCommandsText();

        var prompt = $"The current source contents is:\n{fileContentsText}\n";
        if (!string.IsNullOrWhiteSpace(compileErrors))
        {
            prompt += $"The current compile errors are:\n{compileErrors}\n";
        }
        prompt += $"The user prompt was:\n{userPromptText}\n";
        prompt += $"Please choose actions to solve the user prompt:\n{mcpText}";
        return prompt;
    }

    public void ProcessResponse(string responseText)
        => fileRepository.ProcessResponse(responseText);
}