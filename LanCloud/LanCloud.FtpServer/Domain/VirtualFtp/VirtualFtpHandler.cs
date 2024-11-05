using LanCloud.Shared.Log;
using LanCloud.Models;
using LanCloud.Servers.Ftp.Interfaces;
using LanCloud.Domain.Application;
using LanCloud.Domain.IO;
using System;
using System.IO;

namespace LanCloud.Domain.VirtualFtp
{
    public class VirtualFtpHandler : IFtpHandler
    {
        public VirtualFtpHandler(LocalApplication application, ILogger logger)
        {
            Application = application;
            Logger = logger;

            //Logger.Info($"Loaded");
        }

        public ILogger Logger { get; }
        public LocalApplication Application { get; }

        public int BufferSize => Constants.BufferSize;

        public IFtpUser ValidateUser(string userName, string password)
            => Application.Authentication.ValidateUser(userName, password);

        public IFtpDirectory[] EnumerateDirectories(string path) 
            => new FtpDirectoryInfo(Application, path, Logger).GetDirectories();
        public IFtpFile[] EnumerateFiles(string path)
            => new FtpDirectoryInfo(Application, path, Logger).GetFiles();

        public void CreateDirectory(string path)
            => new FtpDirectoryInfo(Application, path, Logger).Create();
        public void DeleteDirectory(string path) 
            => new FtpDirectoryInfo(Application, path, Logger).Delete();
        public bool DirectoryExists(string path) 
            => new FtpDirectoryInfo(Application, path, Logger).Exists;
        public void DirectoryMove(string renameFrom, string renameTo)
        {
            var from = new FtpDirectoryInfo(Application, renameFrom, Logger);
            var to = new FtpDirectoryInfo(Application, renameTo, Logger);
            from.MoveTo(to);
        }

        public bool FileExists(string path) 
            => new FtpFileInfo(Application, path, Logger).Exists;
        public void FileDelete(string path)
            => new FtpFileInfo(Application, path, Logger).Delete();
        public void FileMove(string renameFrom, string renameTo)
        {
            var from = new FtpFileInfo(Application, renameFrom, Logger);
            var to = new FtpFileInfo(Application, renameTo, Logger);
            from.Move(to);
        }
        public DateTime FileGetLastWriteTime(string path) 
            => new FtpFileInfo(Application, path, Logger).LastWriteTime;

        public Stream FileOpenRead(string path)
            => new FtpFileInfo(Application, path, Logger).OpenRead();
        public Stream FileOpenWriteCreate(string path)
            => new FtpFileInfo(Application, path, Logger).Create();
        public Stream FileOpenWriteAppend(string path) 
            => new FtpFileInfo(Application, path, Logger).OpenAppend();
    }
}
