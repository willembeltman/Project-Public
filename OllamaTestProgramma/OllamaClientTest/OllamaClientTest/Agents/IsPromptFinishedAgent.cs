using OllamaAgentGenerator.Services;

namespace OllamaAgentGenerator.Agents;

public class IsPromptFinishedAgent(
    string userPromptText, 
    FileRepositoryService fileRepository)
{
    public bool IsDone { get; internal set; }

    public string GeneratePrompt(string compileErrors)
    {
        var fileContentsText = fileRepository.GenerateFileContentsText();
        var prompt = $"The current source contents is:\n{fileContentsText}\n";

        if (!string.IsNullOrWhiteSpace(compileErrors))
        {
            prompt += $"The current compile errors are:\n{compileErrors}\n\n";
        }

        prompt += $"The user prompt is:\n{userPromptText}\n";
        prompt += $"Is the prompt satisfied? Reply [YES] or [NO]";
        return prompt;
    }

    public void ProcessResponse(string responseText)
    {
        if (responseText.Contains("[NO]", StringComparison.InvariantCultureIgnoreCase))
        {
            IsDone = false;
        }
        else if (responseText.Contains("[YES]", StringComparison.InvariantCultureIgnoreCase))
        {
            IsDone = true;
        }
    }
}