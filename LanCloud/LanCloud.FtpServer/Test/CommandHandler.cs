using LanCloud.FtpServer.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LanCloud.Application
{
    internal class CommandHandler : ICommandHandler
    {
        public CommandHandler(string root)
        {
            Root = root;
        }

        public string Root { get; }

        private bool IsPathValid(string path)
        {
            return path.StartsWith(Root);
        }
        private string NormalizeFilename(string path)
        {
            if (path == null)
            {
                path = string.Empty;
            }

            if (path == "/")
            {
                return Root;
            }
            else if (path.StartsWith("/"))
            {
                path = new FileInfo(Path.Combine(Root, path.Substring(1))).FullName;
            }
            else
            {
                throw new Exception("Not valid path");
            }

            return IsPathValid(path) ? path : null;
        }

        public IEnumerable<IDirectoryInfo> EnumerateDirectories(string pathname)
        {
            pathname = NormalizeFilename(pathname);
            var dirinfo = new DirectoryInfo(pathname);
            var directories = dirinfo.GetDirectories();
            return directories
                .Select(directoryInfo => new TestDirectory(directoryInfo));
        }

        public IEnumerable<IFileInfo> EnumerateFiles(string pathname)
        {
            pathname = NormalizeFilename(pathname);
            var dirinfo = new DirectoryInfo(pathname);
            var fileInfos = dirinfo.GetFiles();
            return fileInfos
                .Select(fileInfo => new TestFile(fileInfo));
        }

        public void CreateDirectory(string pathname)
        {
            pathname = NormalizeFilename(pathname);
            Directory.CreateDirectory(pathname);
        }
        public void DeleteDirectory(string pathname)
        {
            pathname = NormalizeFilename(pathname);
            Directory.Delete(pathname);
        }
        public bool DirectoryExists(string pathname)
        {
            pathname = NormalizeFilename(pathname);
            return Directory.Exists(pathname);
        }
        public void DirectoryMove(string renameFrom, string renameTo)
        {
            renameFrom = NormalizeFilename(renameFrom);
            renameTo = NormalizeFilename(renameTo);
            Directory.Move(renameFrom, renameTo);
        }

        public bool FileExists(string pathname)
        {
            pathname = NormalizeFilename(pathname);
            return File.Exists(pathname);
        }
        public void FileDelete(string pathname)
        {
            pathname = NormalizeFilename(pathname);
            File.Delete(pathname);
        }
        public void FileMove(string renameFrom, string renameTo)
        {
            renameFrom = NormalizeFilename(renameFrom);
            renameTo = NormalizeFilename(renameTo);
            File.Move(renameFrom, renameTo);
        }
        public DateTime FileGetLastWriteTime(string pathname)
        {
            pathname = NormalizeFilename(pathname);
            return File.GetLastWriteTime(pathname);
        }

        public Stream FileOpenRead(string pathname)
        {
            pathname = NormalizeFilename(pathname);
            return File.OpenRead(pathname);
        }
        public Stream FileOpenWriteCreate(string pathname)
        {
            pathname = NormalizeFilename(pathname);
            return File.Create(pathname);
        }
        public Stream FileOpenWriteAppend(string pathname)
        {
            pathname = NormalizeFilename(pathname);
            return File.Open(pathname, FileMode.Append);
        }

    }
}
