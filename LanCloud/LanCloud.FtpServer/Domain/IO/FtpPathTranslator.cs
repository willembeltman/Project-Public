using LanCloud.Domain.Application;
using System.IO;
using System.Linq;

namespace LanCloud.Domain.IO
{
    public static class FtpPathTranslator
    {
        public static string TranslateDirectoryPathToFullName(LocalApplication application, string directoryPath)
        {
            if (directoryPath == null || directoryPath == "" || directoryPath == "//")
                directoryPath = "/";

            directoryPath = directoryPath.Replace("/", "\\");
            return application.RootDirectory + directoryPath;
        }
        public static string TranslateDirectoryFullNameToPath(LocalApplication application, DirectoryInfo dirInfo)
        {
            var rootInfo = new DirectoryInfo(application.Config.FileDatabaseDirectoryName);
            var pathFullname = dirInfo.FullName.Substring(rootInfo.FullName.Length);
            var path = pathFullname.Replace("\\", "/");
            return path;
        }

        public static string TranslatePathToFullName(LocalApplication application, string path)
        {
            if (path == null || path == "" || path == "//")
                path = "/";

            path = path.Replace("/", "\\");
            return application.RootDirectory + path + ".fileref";
        }
        public static string TranslateFullnameToPath(LocalApplication application, FileInfo fileInfo)
        {
            var rootInfo = new DirectoryInfo(application.Config.FileDatabaseDirectoryName);
            var pathFullnameWithExtention = fileInfo.FullName.Substring(rootInfo.FullName.Length);
            var pathFullname = pathFullnameWithExtention.Substring(0, pathFullnameWithExtention.Length - ".fileref".Length);
            var path = pathFullname.Replace("\\", "/");
            return path;
        }

        public static string TranslatePathToExtention(LocalApplication application, string path)
        {
            var name = TranslatePathToName(application, path);
            var exts = name.Split('.');
            var ext = exts.Last();
            return ext;
        }

        public static string TranslatePathToName(LocalApplication application, string path)
        {
            var dirs = path.Split('/');
            var name = dirs.Last();
            return name;
        }
    }
}