namespace MyCodingAgent.Models;

public class WorkspaceTask(
    string id, 
    string content)
{
    public string Id { get; set; } = id;
    public string Content { get; set; } = content;
}