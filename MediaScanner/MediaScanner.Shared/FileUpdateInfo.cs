namespace MediaScanner.Shared;

public class FileUpdateInfo
{
    public FileUpdateType UpdateType { get; set; }
    public MediaFileDto File { get; set; } = null!;
}
