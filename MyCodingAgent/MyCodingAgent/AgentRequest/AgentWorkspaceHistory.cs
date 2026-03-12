namespace MyCodingAgent.AgentRequest;

public record AgentWorkspaceHistory(AgentResponse response, string? parseError, AgentActionResult[] actions);
