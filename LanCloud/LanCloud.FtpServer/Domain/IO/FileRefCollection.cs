using LanCloud.Domain.Application;
using LanCloud.Services;
using LanCloud.Shared.Log;
using System.IO;

namespace LanCloud.Domain.IO
{
    public class FileRefCollection
    {
        public FileRefCollection(LocalApplication application, ILogger logger)
        {
            Application = application;
            Logger = logger;

            var rootInfo = new DirectoryInfo(Application.Config.RefDirectoryName);
            if (!rootInfo.Exists) { rootInfo.Create(); }
            RootDirectory = rootInfo.FullName.TrimEnd('\\');
            RootDirectoryInfo = new DirectoryInfo(RootDirectory);

            Reload();
        }

        private void Reload()
        {
            Root = new FileRefDirectory(this, RootDirectoryInfo, Logger);
        }

        public string RootDirectory { get; }
        public DirectoryInfo RootDirectoryInfo { get; }
        public LocalApplication Application { get; }
        public ILogger Logger { get; }

        public FileRefDirectory Root { get; private set; }

        public FileRef Load(string path, FileInfo realInfo)
        {
            // Todo proberen uit cache te halen
            return FileRefService.Load(realInfo);
        }

        public FileRef Save(string path, FileInfo realInfo, FileRef value)
        {
            // Todo proberen uit cache te halen
            var res = FileRefService.Save(realInfo, value);
            Reload();
            return res;
        }
    }
}
