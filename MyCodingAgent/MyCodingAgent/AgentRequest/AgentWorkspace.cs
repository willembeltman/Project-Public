namespace MyCodingAgent.AgentRequest;

public record AgentWorkspace(
    AgentWorkspaceFileBrowser? file_browser,
    AgentWorkspaceFile[] all_files,
    string compiler_output,
    string? search_text,
    AgentWorkspaceSearchResult[] search_results,
    AgentWorkspaceTask[] tasks,
    AgentWorkspaceNote[] notes,
    AgentWorkspaceHistory[] history);
