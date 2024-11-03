using LanCloud.Shared.Log;
using LanCloud.Models;
using LanCloud.Servers.Ftp.Interfaces;
using LanCloud.Domain.Application;
using LanCloud.Domain.IO;
using System;
using System.IO;

namespace LanCloud.Domain.VirtualFtp
{
    public class LocalVirtualFtpHandler : IFtpHandler
    {
        public LocalVirtualFtpHandler(LocalApplication application, ILogger logger)
        {
            Application = application;
            Logger = logger;

            Logger.Info($"Loaded");
        }

        public ILogger Logger { get; }
        public LocalApplication Application { get; }

        public int BufferSize => Constants.BufferSize;

        public IFtpUser ValidateUser(string userName, string password)
            => Application.Authentication.ValidateUser(userName, password);

        public IFtpDirectory[] EnumerateDirectories(string path) 
            => new VirtualDirectoryInfo(Application, path).GetDirectories();
        public IFtpFile[] EnumerateFiles(string path)
            => new VirtualDirectoryInfo(Application, path).GetFiles();

        public void CreateDirectory(string path)
            => new VirtualDirectoryInfo(Application, path).Create();
        public void DeleteDirectory(string path) 
            => new VirtualDirectoryInfo(Application, path).Delete();
        public bool DirectoryExists(string path) 
            => new VirtualDirectoryInfo(Application, path).Exists;
        public void DirectoryMove(string renameFrom, string renameTo)
        {
            var from = new VirtualDirectoryInfo(Application, renameFrom);
            var to = new VirtualDirectoryInfo(Application, renameTo);
            from.MoveTo(to);
        }

        public bool FileExists(string path) 
            => new VirtualFileInfo(Application, path).Exists;
        public void FileDelete(string path)
            => new VirtualFileInfo(Application, path).Delete();
        public void FileMove(string renameFrom, string renameTo)
        {
            var from = new VirtualFileInfo(Application, renameFrom);
            var to = new VirtualFileInfo(Application, renameTo);
            from.Move(to);
        }
        public DateTime FileGetLastWriteTime(string path) 
            => new VirtualFileInfo(Application, path).LastWriteTime;

        public Stream FileOpenRead(string path)
            => new VirtualFileInfo(Application, path).OpenRead();
        public Stream FileOpenWriteCreate(string path)
            => new VirtualFileInfo(Application, path).Create();
        public Stream FileOpenWriteAppend(string path) 
            => new VirtualFileInfo(Application, path).OpenAppend();
    }
}
