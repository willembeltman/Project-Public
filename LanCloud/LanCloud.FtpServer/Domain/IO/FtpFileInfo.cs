using LanCloud.Domain.Application;
using LanCloud.Models;
using LanCloud.Services;
using LanCloud.Shared.Log;
using System;
using System.IO;

namespace LanCloud.Domain.IO
{
    public class FtpFileInfo : IFtpFile
    {
        public FtpFileInfo(LocalApplication application, string path, ILogger logger)
        {
            Application = application;
            Path = path;
            Logger = logger;
            var realFullName = FtpPathTranslator.TranslatePathToFullName(application, path);
            RealFileInfo = new FileInfo(realFullName);
        }
        public FtpFileInfo(LocalApplication application, FileInfo realInfo, ILogger logger)
        {
            Application = application;
            RealFileInfo = realInfo;
            Logger = logger;
            Path = FtpPathTranslator.TranslateFullnameToPath(application, realInfo);
        }

        public LocalApplication Application { get; }
        public string Path { get; }
        public ILogger Logger { get; }

        public string Name => FtpPathTranslator.TranslatePathToName(Path);
        public string Extention => FtpPathTranslator.TranslatePathToExtention(Path);
        private FileInfo RealFileInfo { get; }

        FileRef _FileRef { get; set; }
        public FileRef FileRef
        {
            get
            {
                return _FileRef = _FileRef ?? FileRefService.Load(RealFileInfo);
            }
            set
            {
                _FileRef = FileRefService.Save(RealFileInfo, value);
            }
        }

        public DateTime LastWriteTime => RealFileInfo.LastWriteTime;
        public bool Exists => RealFileInfo.Exists;
        public long? Length => FileRef?.Length;

        public FtpStreamWriter Create()
        {
            // FileRef aanmaken
            FileRef = new FileRef(this);
            return new FtpStreamWriter(this, Logger);
        }

        public FtpStreamReader OpenRead()
        {
            // FileRef moet bestaan
            if (FileRef == null) return null;
            return new FtpStreamReader(this, Logger);
        }

        public FtpStreamAppender OpenAppend()
        {
            // FileRef moet bestaan
            if (FileRef == null) return null;
            return new FtpStreamAppender(this, Logger);
        }

        public void Move(FtpFileInfo to)
        {
            File.Move(RealFileInfo.FullName, to.RealFileInfo.FullName);
        }
        public void Delete()
        {
            throw new NotImplementedException();
        }
    }
}
