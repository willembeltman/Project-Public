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

        public IFtpDirectoryInfo[] EnumerateDirectories(string path) 
            => new PathDirectoryInfo(Application, path, Logger).GetDirectories();
        public IFtpFileInfo[] EnumerateFiles(string path)
            => new PathDirectoryInfo(Application, path, Logger).GetFiles();

        public void CreateDirectory(string path)
            => new PathDirectoryInfo(Application, path, Logger).Create();
        public void DeleteDirectory(string path) 
            => new PathDirectoryInfo(Application, path, Logger).Delete();
        public bool DirectoryExists(string path) 
            => new PathDirectoryInfo(Application, path, Logger).Exists;
        public void DirectoryMove(string renameFrom, string renameTo)
        {
            var from = new PathDirectoryInfo(Application, renameFrom, Logger);
            from.MoveTo(renameTo);
        }

        public bool FileExists(string path) 
            => new PathInfo(Application, path, Logger).Exists;
        public void FileDelete(string path)
            => new PathInfo(Application, path, Logger).Delete();
        public void FileMove(string renameFrom, string renameTo)
        {
            var from = new PathInfo(Application, renameFrom, Logger);
            from.MoveTo(renameTo);
        }
        public DateTime FileGetLastWriteTime(string path) 
            => new PathInfo(Application, path, Logger).LastWriteTime;

        public Stream FileOpenRead(string path)
            => new PathInfo(Application, path, Logger).OpenRead();
        public Stream FileOpenWriteCreate(string path)
            => new PathInfo(Application, path, Logger).Create();
        public Stream FileOpenWriteAppend(string path) 
            => new PathInfo(Application, path, Logger).OpenAppend();
    }
}
