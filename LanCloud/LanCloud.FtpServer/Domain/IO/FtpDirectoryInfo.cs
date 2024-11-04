using LanCloud.Domain.Application;
using LanCloud.Models;
using System;
using System.IO;
using System.Linq;

namespace LanCloud.Domain.IO
{
    public class FtpDirectoryInfo : IFtpDirectory
    {
        public FtpDirectoryInfo(LocalApplication application, string path)
        {
            Application = application;
            Path = path;
            var realFullName = FtpPathTranslator.TranslateDirectoryPathToFullName(application, path);
            RealInfo = new DirectoryInfo(realFullName);
        }
        public FtpDirectoryInfo(LocalApplication application, DirectoryInfo realInfo)
        {
            Application = application;
            RealInfo = realInfo;
            Path = FtpPathTranslator.TranslateDirectoryFullNameToPath(application, realInfo);
        }

        public LocalApplication Application { get; }
        public string Path { get; }
        private DirectoryInfo RealInfo { get; }

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
                .Select(dirinfo => new FtpDirectoryInfo(Application, dirinfo))
                .ToArray();
        public FtpFileInfo[] GetFiles() 
            => RealInfo
                .GetFiles("*.fileref")
                .Select(realInfo => new FtpFileInfo(Application, realInfo))
                .ToArray();
    }
}
