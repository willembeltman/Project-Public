namespace MyCodingAgent.Agents;

public class IsFinishedAgent(
    string userPromptText, 
    Workspace workspace)
{
    public bool IsDone { get; internal set; }
    public bool HasAnswered { get; internal set; }

    public string GeneratePrompt()
    {
        //fileRepository.InitializeFileTracking();

        string fileContentsText = workspace.Files.Count == 0
            ? "<No files in directory>"
            : string.Join(Environment.NewLine, workspace.Files);

        var prompt = $"The current source contents is:\n{fileContentsText}\n";

        if (!string.IsNullOrWhiteSpace(workspace.CompileResult))
        {
            prompt += $"The current compile result is:\n{workspace.CompileResult}\n\n";
        }

        prompt += $"The user prompt is:\n{userPromptText}\n";
        prompt += $"Is the prompt satisfied? Reply [YES] or [NO]";
        return prompt;
    }

    public bool ProcessResponse(string responseText)
    {
        if (responseText.Contains("[NO]", StringComparison.InvariantCultureIgnoreCase))
        {
            IsDone = false;
            return true;
        }
        else if (responseText.Contains("[YES]", StringComparison.InvariantCultureIgnoreCase))
        {
            IsDone = true;
            return true;
        }
        return false;
    }
}