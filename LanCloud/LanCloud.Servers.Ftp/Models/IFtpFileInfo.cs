using System;
using System.IO;

namespace LanCloud.Models
{
    public interface IFtpFileInfo
    {
        string Path { get; }
        string Name { get; }
        long? Length { get; }
        DateTime LastWriteTime { get; }
        string Extention { get; }
        bool Exists { get; }

        Stream Create();
        void Delete();
        Stream OpenAppend();
        Stream OpenRead();
        void MoveTo(string toPath);
    }
}