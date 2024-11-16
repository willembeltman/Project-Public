using LanCloud.Domain.Application;
using LanCloud.Domain.IO;
using LanCloud.Models;
using LanCloud.Shared.Log;
using System;
using System.IO;
using System.Linq;

namespace LanCloud.Domain.VirtualFtp
{
    public class FileRefDirectoryInfo : IFtpDirectoryInfo
    {
        public FileRefDirectoryInfo(LocalApplication application, string path, ILogger logger)
        {
            Application = application;
            Path = path;
            Logger = logger;
            var realFullName = PathTranslator.TranslateDirectoryPathToFullName(application.FileRefs.Root, path);
            RealInfo = new DirectoryInfo(realFullName);
        }
        public FileRefDirectoryInfo(LocalApplication application, DirectoryInfo realInfo, ILogger logger)
        {
            Application = application;
            RealInfo = realInfo;
            Logger = logger;
            Path = PathTranslator.TranslateDirectoryFullNameToPath(application.FileRefs.Root, realInfo);
        }

        public LocalApplication Application { get; }
        public string Path { get; }
        private DirectoryInfo RealInfo { get; }
        public ILogger Logger { get; }

        public string Name => RealInfo.Name;
        public DateTime LastWriteTime => RealInfo.LastWriteTime;
        public void Create() => RealInfo.Create();
        public bool Exists => RealInfo.Exists;
        public void MoveTo(string pathTo)
        {
            var to = PathTranslator.TranslatePathToFullName(Application.FileRefs.Root, pathTo);
            RealInfo.MoveTo(to);
        }

        public IFtpDirectoryInfo[] GetDirectories()
            => RealInfo
                .GetDirectories()
                .Select(dirinfo => new FileRefDirectoryInfo(Application, dirinfo, Logger))
                .ToArray();
        public IFtpFileInfo[] GetFiles()
            => RealInfo
                .GetFiles("*.fileref")
                .Select(realInfo => new FileRefInfo(Application, realInfo, Logger))
                .ToArray();

        public void Delete()
        {
            RealInfo.Delete(false);

            Application.FileRefs.DeleteDirectory(RealInfo);

            //Application.FileRefs.Reload();
        }
    }
}
