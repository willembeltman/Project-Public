using LanCloud.Domain.Application;
using LanCloud.Domain.IO;
using LanCloud.Domain.IO.Readers;
using LanCloud.Domain.IO.Writers;
using LanCloud.Models;
using LanCloud.Shared.Log;
using System;
using System.IO;

namespace LanCloud.Domain.VirtualFtp
{
    public class PathFileInfo : IFtpFileInfo
    {
        public PathFileInfo(LocalApplication application, string path, ILogger logger)
        {
            Application = application;
            Path = path;
            Logger = logger;
            var realFullName = PathTranslator.TranslatePathToFullName(application.FileRefs.Root, path);
            RealInfo = new FileInfo(realFullName);
        }
        public PathFileInfo(LocalApplication application, FileInfo realInfo, ILogger logger)
        {
            Application = application;
            RealInfo = realInfo;
            Logger = logger;
            Path = PathTranslator.TranslateFullnameToPath(application.FileRefs.Root, realInfo);
        }

        public LocalApplication Application { get; }
        public string Path { get; }
        public ILogger Logger { get; }
        public FileInfo RealInfo { get; }

        public string Name => PathTranslator.TranslatePathToName(Path);
        public string Extention => PathTranslator.TranslatePathToExtention(Path);

        FileRef _FileRef { get; set; }
        public FileRef FileRef
        {
            get
            {
                return _FileRef = _FileRef ?? Application.FileRefs.Load(Path, RealInfo);
            }
            set
            {
                _FileRef = Application.FileRefs.Save(Path, RealInfo, value);
            }
        }

        public DateTime LastWriteTime => RealInfo.LastWriteTime;
        public bool Exists => RealInfo.Exists;
        public long? Length => FileRef?.Length;

        public Stream Create()
        {
            FileRef = new FileRef(this);
            return new FileRefWriter(this, Logger);
        }

        public Stream OpenRead()
        {
            if (FileRef == null) return null;
            return new FileRefReader(this, Logger);
        }

        public Stream OpenAppend()
        {
            if (FileRef == null) return null;
            return new FileRefAppender(this, Logger);
        }

        public void MoveTo(string toPath)
        {
            if (FileRef == null) return;
            var to = new PathFileInfo(Application, toPath, Logger);
            Application.FileRefs.Move(RealInfo, Extention, FileRef, to.RealInfo, to.Extention);
        }
        public void Delete()
        {
            if (FileRef == null) return;
            Application.FileRefs.Delete(RealInfo, Extention, FileRef);
        }
    }
}
