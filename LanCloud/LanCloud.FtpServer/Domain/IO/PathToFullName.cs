using LanCloud.Domain.Application;

namespace LanCloud.Domain.IO
{
    public static class PathToFullName
    {
        public static string Translate(LocalApplication application, string path)
        {
            if (path == null || path == "" || path == "//")
                path = "/";

            path = path.Replace("/", "\\");
            var root = application.Config.FileDatabaseDirectoryName;
            return System.IO.Path.Combine(root, path);
        }
    }
}