using LanCloud.Domain.VirtualFtp;
using LanCloud.Services;
using LanCloud.Shared.Log;
using System.IO;
using System.Linq;

namespace LanCloud.Domain.IO
{
    public class FileRefDirectory
    {
        public FileRefDirectory(FileRefCollection fileRefCollection, DirectoryInfo realInfo, ILogger logger)
        {
            FileRefCollection = fileRefCollection;
            RealInfo = realInfo;
            Logger = logger;

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

        public FileRefCollection FileRefCollection { get; }
        public DirectoryInfo RealInfo { get; }
        public ILogger Logger { get; }

        public string Path { get; }
        public FileRef[] FileRefs { get; }
        public FileRefDirectory[] FileRefDirectories { get; }

        public string Name => PathTranslator.TranslatePathToName(Path);

    }
}
