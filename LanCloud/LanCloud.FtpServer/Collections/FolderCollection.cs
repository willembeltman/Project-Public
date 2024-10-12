using LanCloud.Ftp;
using LanCloud.Ftp.Interfaces;
using LanCloud.Handlers;
using LanCloud.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;

namespace LanCloud
{
    internal class FolderCollection : IEnumerable<Folder>, IFtpHandler, IDisposable
    {
        public FolderCollection(ApplicationConfig config)
        {
            Url = config.Url;
            var port = config.Port;
            FolderHandlers = config.Folders
                .Select(dataFolder => new Folder(IPAddress.Any, port++, dataFolder))
                .ToArray();
        }

        public string Url { get; }
        public Folder[] FolderHandlers { get; }

        public void CreateDirectory(string pathname)
        {
            throw new NotImplementedException();
        }

        public void DeleteDirectory(string pathname)
        {
            throw new NotImplementedException();
        }

        public bool DirectoryExists(string pathname)
        {
            throw new NotImplementedException();
        }

        public void DirectoryMove(string renameFrom, string renameTo)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<FtpDirectory> EnumerateDirectories(string pathname)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<FtpFile> EnumerateFiles(string pathname)
        {
            throw new NotImplementedException();
        }

        public void FileDelete(string pathname)
        {
            throw new NotImplementedException();
        }

        public bool FileExists(string pathname)
        {
            throw new NotImplementedException();
        }

        public DateTime FileGetLastWriteTime(string pathname)
        {
            throw new NotImplementedException();
        }

        public void FileMove(string renameFrom, string renameTo)
        {
            throw new NotImplementedException();
        }

        public Stream FileOpenRead(string pathname)
        {
            throw new NotImplementedException();
        }

        public Stream FileOpenWriteAppend(string pathname)
        {
            throw new NotImplementedException();
        }

        public Stream FileOpenWriteCreate(string pathname)
        {
            throw new NotImplementedException();
        }

        public FtpUser ValidateUser(string userName, string password)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<Folder> GetEnumerator()
        {
            return ((IEnumerable<Folder>)FolderHandlers).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return FolderHandlers.GetEnumerator();
        }

        public void Dispose()
        {
            foreach (var item in FolderHandlers)
                item.Dispose();
        }
    }
}