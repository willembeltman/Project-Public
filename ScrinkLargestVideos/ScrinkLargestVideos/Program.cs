using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrinkLargestVideos
{
    class Program
    {
        static string Path = @"E:\Videoedit";

        public static string[] Extentions = { ".mp4", ".mkv", ".mov" };

        static void Main(string[] args)
        {
            using (var db = new ApplicationDbContext())
            {
                SyncFiles(db);
                var list = db.FileRapports
                    .Select(file =>
                    {
                        if (!file.FullName.StartsWith(@"E:\Videoedit\Videos\")) return null;
                        if (!File.Exists(file.FullName)) return null;
                        if (file.Length < 1024 * 1024 * 1024) return null;
                        if (file.Rapport.streams == null) return null;
                        var videostream = file.Rapport.streams.FirstOrDefault(b => b.codec_type == "video");
                        if (videostream == null) return null;
                        var pixelCount = videostream.width * videostream.height;
                        var split = videostream.avg_frame_rate.Split('/');
                        var baseFps = Convert.ToInt64(split[0]);
                        var diverdFps = Convert.ToInt64(split[1]);
                        var fps = Convert.ToDouble(baseFps) / diverdFps;
                        var duration = Convert.ToDouble(file.Rapport.format.duration);
                        var sizePerPixel = file.Length / fps / duration / pixelCount;
                        return new { FullName = file.FullName, Size = file.Length / 1024 / 1024, Duration = duration, SizePerPixel = sizePerPixel };
                    })
                    .Where(a => a != null)
                    .OrderByDescending(a => a.SizePerPixel)
                    .ToArray();

                foreach (var item in list)
                {

                }
            }
        }

        private static void SyncFiles(ApplicationDbContext db)
        {
            var dirinfo = new DirectoryInfo(Path);
            var list = Search(dirinfo);
            var memList = new List<FileInfo>();
            foreach (var file in list)
            {
                var dbFile = db.FileRapports.FirstOrDefault(a => a.FullName == file.FullName);
                if (dbFile == null)
                {
                    var newFile = new FileRapport(file);
                    db.FileRapports.Add(newFile);
                    Console.WriteLine(file.FullName);
                }
                memList.Add(file);
            }
            foreach (var file in db.FileRapports)
            {
                var dbFile = memList.FirstOrDefault(a => a.FullName == file.FullName);
                if (dbFile == null)
                {
                    db.FileRapports.Remove(file);
                    Console.WriteLine($"{file.FullName} removed");
                }
            }
        }

        private static IEnumerable<FileInfo> Search(DirectoryInfo dirinfo)
        {
            var files = dirinfo
                .GetFiles()
                .OrderBy(a => a.Name);
            foreach (var file in files)
            {
                if (Extentions.Contains(file.Extension.ToLower()))
                    yield return file;
            }

            var subdirs = dirinfo
                .GetDirectories()
                .OrderBy(a => a.Name);
            foreach (var subdir in subdirs)
            {
                var subdirfiles = Search(subdir);
                foreach (var subdirfile in subdirfiles)
                {
                    yield return subdirfile;
                }
            }
        }
    }
}
