namespace ScrinkLargestVideos;

class Program
{
    static void Main(string[] args)
    {
        var path = @"E:\Videoedit";
        string[] extentions = [".mp4", ".mkv", ".mov"];
        var cts = new CancellationTokenSource();
        var db = new ApplicationDbContext();
        var task = async () =>
        {
            SyncFiles(db, path, extentions, cts.Token);
            var list = db.FileRapports
                .Select(file =>
                {
                    //if (!file.FullName.StartsWith(@"E:\Videoedit\Videos\")) return null;
                    if (!File.Exists(file.FullName)) return null;
                    if (file.Length < 1024 * 1024 * 1024) return null;
                    var rapport = file.GetRapport();
                    if (rapport.Streams == null) return null;
                    var videostream = rapport.Streams.FirstOrDefault(b => b.codec_type == "video");
                    if (videostream == null) return null;
                    var pixelCount = videostream.width * videostream.height;
                    var split = videostream.avg_frame_rate.Split('/');
                    var baseFps = Convert.ToInt64(split[0]);
                    var diverdFps = Convert.ToInt64(split[1]);
                    var fps = Convert.ToDouble(baseFps) / diverdFps;
                    var duration = Convert.ToDouble(rapport.Format.Duration.Replace(".", ","));
                    var sizePerPixel = file.Length / fps / duration / pixelCount;
                    return new { FullName = file.FullName, Size = file.Length / 1024 / 1024, Duration = duration, SizePerPixel = sizePerPixel };
                })
                .Where(a => a != null)
                .OrderByDescending(a => a!.SizePerPixel)
                .ToArray();

            foreach (var item in list)
            {

            }
        };

        Task.Run(task, cts.Token);

        while (true)
        {
            var key = Console.ReadKey();
            if (key.Key == ConsoleKey.Q) break;
        }

        cts.Cancel();
        cts.Dispose();
    }

    private static void SyncFiles(ApplicationDbContext db, string path, string[] extentions, CancellationToken ct)
    {
        var dirinfo = new DirectoryInfo(path);
        var list = Search(dirinfo, extentions, ct);
        var memList = new List<FileInfo>();
        foreach (var file in list)
        {
            var dbFile = db.FileRapports.FirstOrDefault(a => a.FullName == file.FullName);
            if (dbFile == null)
            {
                var newFile = new FileRapport(file);
                db.FileRapports.Add(newFile);
                Console.WriteLine($"{file.FullName} Added!");
                db.SaveChanges();
            }
            else
            {
                Console.WriteLine($"{file.FullName} skipped");
            }
            memList.Add(file);

            if (ct.IsCancellationRequested) return; 
        }
        foreach (var file in db.FileRapports)
        {
            var dbFile = memList.FirstOrDefault(a => a.FullName == file.FullName);
            if (dbFile == null)
            {
                db.FileRapports.Remove(file);
                Console.WriteLine($"{file.FullName} removed");
            }

            if (ct.IsCancellationRequested) return;
        }
    }
    private static IEnumerable<FileInfo> Search(DirectoryInfo dirinfo, string[] extentions, CancellationToken ct)
    {
        var files = dirinfo
            .GetFiles()
            .OrderBy(a => a.Name);
        foreach (var file in files)
        {
            if (extentions.Contains(file.Extension.ToLower()))
                yield return file;
            if (ct.IsCancellationRequested) yield break;
        }

        var subdirs = dirinfo
            .GetDirectories()
            .OrderBy(a => a.Name);
        foreach (var subdir in subdirs)
        {
            var subdirfiles = Search(subdir, extentions, ct);
            foreach (var subdirfile in subdirfiles)
            {
                yield return subdirfile;
                if (ct.IsCancellationRequested) yield break;
            }
            if (ct.IsCancellationRequested) yield break;
        }
    }
}
