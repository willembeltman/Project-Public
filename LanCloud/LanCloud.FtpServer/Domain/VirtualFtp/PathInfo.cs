using LanCloud.Domain.Application;
using LanCloud.Domain.IO;
using LanCloud.Models;
using LanCloud.Services;
using LanCloud.Shared.Log;
using System;
using System.IO;

namespace LanCloud.Domain.VirtualFtp
{
    public class PathInfo : IFtpFileInfo
    {
        public PathInfo(LocalApplication application, string path, ILogger logger)
        {
            Application = application;
            Path = path;
            Logger = logger;
            var realFullName = PathTranslator.TranslatePathToFullName(application.FileRefs.RootDirectoryInfo, path);
            FileRefInfo = new FileInfo(realFullName);
        }
        public PathInfo(LocalApplication application, FileInfo realInfo, ILogger logger)
        {
            Application = application;
            FileRefInfo = realInfo;
            Logger = logger;
            Path = PathTranslator.TranslateFullnameToPath(application.FileRefs.RootDirectoryInfo, realInfo);
        }

        public LocalApplication Application { get; }
        public string Path { get; }
        public ILogger Logger { get; }
        public FileInfo FileRefInfo { get; }

        public string Name => PathTranslator.TranslatePathToName(Path);
        public string Extention => PathTranslator.TranslatePathToExtention(Path);

        FileRef _FileRef { get; set; }
        public FileRef FileRef
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
        public long? Length => FileRef?.Length;

        public Stream Create()
        {
            // FileRef aanmaken
            FileRef = new FileRef(this);
            return new FtpStreamWriter(this, Logger);
        }

        public Stream OpenRead()
        {
            // FileRef moet bestaan
            if (FileRef == null) return null;
            return new FtpStreamReader(this, Logger);
        }

        public Stream OpenAppend()
        {
            // FileRef moet bestaan
            if (FileRef == null) return null;
            return new FtpStreamAppender(this, Logger);
        }

        public void MoveTo(string toPath)
        {
            var to = new PathInfo(Application, toPath, Logger);
            File.Move(FileRefInfo.FullName, to.FileRefInfo.FullName);
        }
        public void Delete()
        {
            throw new NotImplementedException();
        }
    }
}
