namespace MyCodingAgent.AgentRequest;

public record AgentWorkspaceFileBrowser(
    string path,
    Line[] lines);
