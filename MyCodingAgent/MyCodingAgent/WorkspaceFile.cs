namespace MyCodingAgent;

public record WorkspaceFile(
    string RelativePath,
    string RealPath,
    string FileContent);
