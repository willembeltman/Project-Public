using LanCloud.Domain.Application;
using LanCloud.Models;
using LanCloud.Shared.Log;
using System;
using System.IO;
using System.Linq;

namespace LanCloud.Domain.VirtualFtp
{
    public class PathDirectoryInfo : IFtpDirectoryInfo
    {
        public PathDirectoryInfo(LocalApplication application, string path, ILogger logger)
        {
            Application = application;
            Path = path;
            Logger = logger;
            var realFullName = PathTranslator.TranslateDirectoryPathToFullName(application.FileRefs.RootDirectoryInfo, path);
            RealInfo = new DirectoryInfo(realFullName);
        }
        public PathDirectoryInfo(LocalApplication application, DirectoryInfo realInfo, ILogger logger)
        {
            Application = application;
            RealInfo = realInfo;
            Logger = logger;
            Path = PathTranslator.TranslateDirectoryFullNameToPath(application.FileRefs.RootDirectoryInfo, realInfo);
        }

        public LocalApplication Application { get; }
        public string Path { get; }
        private DirectoryInfo RealInfo { get; }
        public ILogger Logger { get; }

        public string Name => RealInfo.Name;
        public DateTime LastWriteTime => RealInfo.LastWriteTime;
        public void Create() => RealInfo.Create();
        public void Delete() => RealInfo.Delete();
        public bool Exists => RealInfo.Exists;
        public void MoveTo(string pathTo)
        {
            var to = PathTranslator.TranslatePathToFullName(Application.FileRefs.RootDirectoryInfo, pathTo);
            RealInfo.MoveTo(to);
        }

        public IFtpDirectoryInfo[] GetDirectories()
            => RealInfo
                .GetDirectories()
                .Select(dirinfo => new PathDirectoryInfo(Application, dirinfo, Logger))
                .ToArray();
        public IFtpFileInfo[] GetFiles()
            => RealInfo
                .GetFiles("*.fileref")
                .Select(realInfo => new PathInfo(Application, realInfo, Logger))
                .ToArray();
    }
}
