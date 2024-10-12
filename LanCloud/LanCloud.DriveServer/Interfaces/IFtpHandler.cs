using LanCloud.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace LanCloud.Ftp.Interfaces
{
    public interface IFtpHandler
    {
        FtpUser ValidateUser(string userName, string password);

        IEnumerable<FtpDirectory> EnumerateDirectories(string pathname);
        void CreateDirectory(string pathname);
        void DeleteDirectory(string pathname);
        bool DirectoryExists(string pathname);
        void DirectoryMove(string renameFrom, string renameTo);

        IEnumerable<FtpFile> EnumerateFiles(string pathname);
        Stream FileOpenRead(string pathname);
        Stream FileOpenWriteCreate(string pathname);
        Stream FileOpenWriteAppend(string pathname);
        void FileDelete(string pathname);
        bool FileExists(string pathname);
        void FileMove(string renameFrom, string renameTo);
        DateTime FileGetLastWriteTime(string pathname);
    }
}