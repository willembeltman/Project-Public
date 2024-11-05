using System;
namespace LanCloud.Models
{
    public interface IFtpDirectoryInfo
    {
        string Path { get; }
        string Name { get; }
        DateTime LastWriteTime { get; }
        bool Exists { get; }

        void Create();
        void Delete();
        IFtpDirectoryInfo[] GetDirectories();
        IFtpFileInfo[] GetFiles();
        void MoveTo(string pathTo);
    }
}