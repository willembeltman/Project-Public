using LanCloud.Domain.Application;
using LanCloud.Models;
using LanCloud.Services;
using System;
using System.IO;

namespace LanCloud.Domain.IO
{
    public class VirtualFileInfo : IFtpFile
    {
        public VirtualFileInfo(LocalApplication application, string path)
        {
            Application = application;
            Path = path;
            var realFullName = PathToFullName.Translate(application, path);
            FileRefInfo = new FileInfo(realFullName);
        }
        public VirtualFileInfo(LocalApplication application, FileInfo realInfo)
        {
            Application = application;
            FileRefInfo = realInfo;
            Path = FullNameToPath.Translate(application, realInfo);
        }

        public LocalApplication Application { get; }
        public string Path { get; }
        public FileInfo FileRefInfo { get; }

        FileRef _FileRef { get; set; }
        FileRef FileRef
        {
            get
            {
                return _FileRef = _FileRef ?? FileRefService.Load(FileRefInfo);
            }
            set
            {
                _FileRef = FileRefService.Save(FileRefInfo, value);
            }
        }

        public DateTime LastWriteTime => FileRefInfo.LastWriteTime;
        public bool Exists => FileRefInfo.Exists;
        public string Name => FileRef.Name;
        public long Length => FileRef.Length;

        public VirtualStreamWriter Create() 
            => new VirtualStreamWriter(this);
        public VirtualStreamAppender OpenAppend()
            => new VirtualStreamAppender(this);
        public VirtualStreamReader OpenRead()
            => new VirtualStreamReader(this);

        public void Move(VirtualFileInfo to)
        {
            throw new NotImplementedException();
        }
        public void Delete()
        {
            throw new NotImplementedException();
        }
    }
}
