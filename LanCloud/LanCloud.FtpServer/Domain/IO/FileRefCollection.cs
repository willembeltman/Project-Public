using LanCloud.Domain.Application;
using LanCloud.Services;
using LanCloud.Shared.Log;
using System;
using System.IO;
using System.Linq;

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

            RootFullName = rootInfo.FullName.TrimEnd('\\');
            Root = new DirectoryInfo(RootFullName);

            Reload();
        }

        public string RootFullName { get; }
        public DirectoryInfo Root { get; }
        public LocalApplication Application { get; }
        public ILogger Logger { get; }

        public FileRefDirectory FileRefRoot { get; private set; }

        private void Reload()
        {
            FileRefRoot = new FileRefDirectory(this, Root, Logger);
        }

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

        public void Delete(FileInfo realInfo, string extention, FileRef fileRef)
        {
            var fileBits = fileRef.Bits
                .SelectMany(a => Application.FindFileBits(extention, fileRef, a)).ToArray();

            foreach (var fileBit in fileBits)
            {
                fileBit.Info.Delete();
            }

            realInfo.Delete();
            Reload();
        }

        public void DeleteDirectory(DirectoryInfo realInfo)
        {
            realInfo.Delete();
            Reload();
        }

        public void Move(FileInfo realInfo, string extention, FileRef fileRef, FileInfo toRealInfo, string toExtention)
        {
            if (extention != toExtention)
            {
                var fileBits = fileRef.Bits
                    .SelectMany(a => Application.FindFileBits(extention, fileRef, a))
                    .Select(a => new { OldFileBit = a, NewFileBit = new FileBit(a.Info.Directory, toExtention, a.Indexes, a.Length, a.Hash)})                    
                    .ToArray();

                foreach (var fileBit in fileBits)
                {
                    fileBit.OldFileBit.Info.MoveTo(fileBit.NewFileBit.Info.FullName);
                }
            }

            File.Move(realInfo.FullName, toRealInfo.FullName);
        }
    }
}
