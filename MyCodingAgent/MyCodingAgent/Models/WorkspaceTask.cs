namespace MyCodingAgent.Models;

public class WorkspaceTask(
    int id, 
    string content)
{
    public int Id { get; set; } = id;
    public string Content { get; set; } = content;
}