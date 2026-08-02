using MediaScanner.Shared.Enums;

namespace MediaScanner.Shared.Models;

public class FileUpdateInfo
{
    public FileUpdateType UpdateType { get; set; }
    public MediaFileDto File { get; set; } = null!;
}
