using LanCloud.Domain.Application;
using System.IO;

namespace LanCloud.Domain.IO
{
    public class FullNameToPath
    {
        public static string Translate(LocalApplication application, DirectoryInfo dirInfo)
        {
            var rootInfo = new DirectoryInfo(application.Config.FileDatabaseDirectoryName);
            return TranslateInner(dirInfo.FullName, rootInfo.FullName);
        }
        public static string Translate(LocalApplication application, FileInfo fileInfo)
        {
            var rootInfo = new DirectoryInfo(application.Config.FileDatabaseDirectoryName);
            return TranslateInner(fileInfo.FullName, rootInfo.FullName);
        }

        private static string TranslateInner(string dirFullName, string rootFullName)
        {
            var pathFullname = dirFullName.Substring(rootFullName.Length);
            var path = pathFullname.Replace("\\", "/");
            return path;
        }
    }
}