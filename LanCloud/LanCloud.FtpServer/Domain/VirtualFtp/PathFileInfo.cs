using LanCloud.Domain.Application;
using LanCloud.Domain.IO;
using LanCloud.Models;
using LanCloud.Shared.Log;
using System;
using System.IO;
using System.Linq;

namespace LanCloud.Domain.VirtualFtp
{
    public class PathFileInfo : IFtpFileInfo
    {
        public PathFileInfo(LocalApplication application, string path, ILogger logger)
        {
            Application = application;
            Path = path;
            Logger = logger;
            var realFullName = PathTranslator.TranslatePathToFullName(application.FileRefs.RootDirectoryInfo, path);
            RealInfo = new FileInfo(realFullName);
        }
        public PathFileInfo(LocalApplication application, FileInfo realInfo, ILogger logger)
        {
            Application = application;
            RealInfo = realInfo;
            Logger = logger;
            Path = PathTranslator.TranslateFullnameToPath(application.FileRefs.RootDirectoryInfo, realInfo);
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
            // FileRef aanmaken
            FileRef = new FileRef(this);
            return new FileRefWriter(this, Logger);
        }

        public Stream OpenRead()
        {
            // FileRef moet bestaan
            if (FileRef == null) return null;
            return new FileRefReader(this, Logger);
        }

        public Stream OpenAppend()
        {
            // FileRef moet bestaan
            if (FileRef == null) return null;
            return new FileRefAppender(this, Logger);
        }

        public void MoveTo(string toPath)
        {
            var to = new PathFileInfo(Application, toPath, Logger);
            File.Move(RealInfo.FullName, to.RealInfo.FullName);
        }
        public void Delete()
        {
            if (FileRef == null) return;

            var fileBits = FileRef.FileRefBits
                .SelectMany(a => Application.FindFileBits(Extention, FileRef, a)).ToArray();
            foreach (var fileBit in fileBits)
            {
                fileBit.Info.Delete();
            }
            RealInfo.Delete();
        }
    }
}
