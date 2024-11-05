using LanCloud.Domain.Application;
using LanCloud.Models;
using LanCloud.Shared.Log;
using System;
using System.IO;
using System.Linq;

namespace LanCloud.Domain.IO
{
    public class FtpDirectoryInfo : IFtpDirectory
    {
        public FtpDirectoryInfo(LocalApplication application, string path, ILogger logger)
        {
            Application = application;
            Path = path;
            Logger = logger;
            var realFullName = FtpPathTranslator.TranslateDirectoryPathToFullName(application, path);
            RealInfo = new DirectoryInfo(realFullName);
        }
        public FtpDirectoryInfo(LocalApplication application, DirectoryInfo realInfo, ILogger logger)
        {
            Application = application;
            RealInfo = realInfo;
            Logger = logger;
            Path = FtpPathTranslator.TranslateDirectoryFullNameToPath(application, realInfo);
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
        public void MoveTo(FtpDirectoryInfo to)
            => RealInfo.MoveTo(to.RealInfo.FullName);

        public FtpDirectoryInfo[] GetDirectories()
            => RealInfo
                .GetDirectories()
                .Select(dirinfo => new FtpDirectoryInfo(Application, dirinfo, Logger))
                .ToArray();
        public FtpFileInfo[] GetFiles()
            => RealInfo
                .GetFiles("*.fileref")
                .Select(realInfo => new FtpFileInfo(Application, realInfo, Logger))
                .ToArray();
    }
}
