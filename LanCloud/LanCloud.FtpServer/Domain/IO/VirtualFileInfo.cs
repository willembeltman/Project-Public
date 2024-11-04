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
            Name = PathTranslator.TranslatePathToName(application, path);
            Extention = PathTranslator.TranslatePathToExtention(application, path);
            var realFullName = PathTranslator.TranslatePathToFullName(application, path);
            RealFileInfo = new FileInfo(realFullName);
        }
        public VirtualFileInfo(LocalApplication application, FileInfo realInfo)
        {
            Application = application;
            RealFileInfo = realInfo;
            Path = PathTranslator.TranslateFullnameToPath(application, realInfo);
        }

        public LocalApplication Application { get; }
        public string Path { get; }
        public string Name { get; }
        public string Extention { get; }
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
        public long? Length
        {
            get => FileRef?.Length;
            set
            {
                var fileRef = new FileRef(this);
                fileRef.Length = value;
                FileRef = fileRef;
            }
        }
        public string Hash
        {
            get => FileRef?.Hash;
            set
            {
                var fileRef = new FileRef(this);
                fileRef.Hash = value;
                FileRef = fileRef;
            }
        }
        public FileRefBit[] FileRefBits
        {
            get => FileRef?.FileRefBits;
            set
            {
                var fileRef = new FileRef(this);
                fileRef.FileRefBits = value;
                FileRef = fileRef;
            }
        }

        public VirtualStreamWriter Create()
        {
            FileRef = new FileRef(this);
            return new VirtualStreamWriter(this);
        }
        public VirtualStreamAppender OpenAppend()
            => new VirtualStreamAppender(this);
        public VirtualStreamReader OpenRead()
            => new VirtualStreamReader(this);

        public void Move(VirtualFileInfo to)
        {
            File.Move(RealFileInfo.FullName, to.RealFileInfo.FullName);
        }
        public void Delete()
        {
            throw new NotImplementedException();
        }
    }
}
