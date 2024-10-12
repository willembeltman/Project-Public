using System;
using System.Collections.Generic;
using System.IO;

namespace LanCloud.FtpServer
{
    public interface ICommandHandler
    {
        void CreateDirectory(string pathname);
        void DeleteDirectory(string pathname);
        bool DirectoryExists(string pathname);
        void DirectoryMove(string renameFrom, string renameTo);
        Stream FileOpenWriteAppend(string pathname);
        Stream FileOpenWriteCreate(string pathname);
        void FileDelete(string pathname);
        bool FileExists(string pathname);
        DateTime FileGetLastWriteTime(string pathname);
        void FileMove(string renameFrom, string renameTo);
        Stream FileOpenRead(string pathname);
        IEnumerable<IDirectoryInfo> EnumerateDirectories(string pathname);
        IEnumerable<IFileInfo> EnumerateFiles(string pathname);
    }
}