using LanCloud.Domain.VirtualFtp;
using LanCloud.Models;
using LanCloud.Services;
using LanCloud.Shared.Log;
using System.IO;
using System.Linq;

namespace LanCloud.Domain.Application
{
    public class FileRefDirectory
    {
        public FileRefDirectory(FileRefCollection fileRefCollection, DirectoryInfo realInfo, ILogger logger)
        {
            Path = PathTranslator.TranslateDirectoryFullNameToPath(fileRefCollection.Root, realInfo);
            FileRefs = realInfo
                .GetFiles("*.fileref")
                .Select(file => FileRefService.Load(file))
                .ToArray();
            FileRefDirectories = realInfo
                .GetDirectories()
                .Select(subdir => new FileRefDirectory(fileRefCollection, subdir, logger))
                .ToArray();
        }

        public string Path { get; }
        public FileRef[] FileRefs { get; }
        public FileRefDirectory[] FileRefDirectories { get; }

        public string Name => PathTranslator.TranslatePathToName(Path);

    }
}
