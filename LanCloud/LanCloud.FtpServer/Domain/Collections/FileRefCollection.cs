using LanCloud.Domain.Application;
using LanCloud.Domain.IO;
using LanCloud.Domain.VirtualFtp;
using LanCloud.Shared.Log;
using System;
using System.IO;
using System.Linq;

namespace LanCloud.Domain.Collections
{
    public class FileRefCollection
    {
        public FileRefCollection(LocalApplication application, ILogger logger)
        {
            Application = application;
            Logger = logger;

            var rootInfo = new DirectoryInfo(Application.Config.RefDirectory);
            if (!rootInfo.Exists) { rootInfo.Create(); }
            RootDirectory = rootInfo.FullName.TrimEnd('\\');
            RootDirectoryInfo = new DirectoryInfo(RootDirectory);

            Root = new FileRefDirectory(this, RootDirectoryInfo, logger);
        }

        public string RootDirectory { get; }
        public DirectoryInfo RootDirectoryInfo { get; }
        public FileRefDirectory Root { get; }
        public LocalApplication Application { get; }
        public ILogger Logger { get; }

        public FileRefDirectory GetDirectory(string path)
        {
            var split = path.Split('/');
            var dir = Root;
            for (int i = 0; i < split.Length; i++)
            {
                var name = split[i];
                var subdir = dir.FileRefDirectories.FirstOrDefault(a => a.Name == name);
                if (subdir != null)
                {
                    dir = subdir;
                }
                else
                {
                    return null;
                }
            }
            return dir;
        }

        internal void Add(PathInfo pathInfo)
        {
            //throw new NotImplementedException();
        }
    }
}
