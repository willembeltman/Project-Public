using LanCloud.Domain.Application;
using LanCloud.Models;
using System;
using System.IO;
using System.Linq;

namespace LanCloud.Domain.IO
{
    public class VirtualDirectoryInfo : IFtpDirectory
    {
        public VirtualDirectoryInfo(LocalApplication application, string path)
        {
            Application = application;
            Path = path;
            var realFullName = PathToFullName.Translate(application, path);
            RealInfo = new DirectoryInfo(realFullName);
        }
        public VirtualDirectoryInfo(LocalApplication application, DirectoryInfo realInfo)
        {
            Application = application;
            RealInfo = realInfo;
            Path = FullNameToPath.Translate(application, realInfo);
        }

        public LocalApplication Application { get; }
        public string Path { get; }
        private DirectoryInfo RealInfo { get; }

        public string Name => RealInfo.Name;
        public DateTime LastWriteTime => RealInfo.LastWriteTime;
        public void Create() => RealInfo.Create();
        public void Delete() => RealInfo.Delete();
        public bool Exists => RealInfo.Exists;
        public void MoveTo(VirtualDirectoryInfo to) 
            => RealInfo.MoveTo(to.RealInfo.FullName);

        public VirtualDirectoryInfo[] GetDirectories()
            => RealInfo
                .GetDirectories()
                .Select(dirinfo => new VirtualDirectoryInfo(Application, dirinfo))
                .ToArray();
        public VirtualFileInfo[] GetFiles() 
            => RealInfo
                .GetFiles()
                .Select(a => new VirtualFileInfo(Application, a))
                .ToArray();
    }
}
