using LanCloud.Domain.Application;
using System.IO;

namespace LanCloud.Domain.IO
{
    public static class PathToFullName
    {
        public static string Translate(LocalApplication application, string path)
        {
            if (path == null || path == "" || path == "//")
                path = "/";

            path = path.Replace("/", "\\");
            return application.RootDirectory + path;
        }
    }
}