using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DoubleFilesChecker
{
    class Program
    {
        static void Main(string[] args)
        {
            var dir = @"E:\Music";
            var dirmoved = @"E:\Double";
            var dirinfo = new DirectoryInfo(dir);
            var files = new List<FileItem>();

            GetFiles(files, dirinfo);

            Console.Clear();

            foreach (var file in files)
            {
                if (file.Found == false)
                {
                    var foundfiles = files
                        .Where(a =>
                            a.Found == false &&
                            //a.FullName != file.FullName &&
                            //a.Name == file.Name &&
                            a.Length == file.Length &&
                            a.Hash.Length == file.Hash.Length &&
                            a.Hash.SequenceEqual(file.Hash))
                        .ToArray();
                    if (foundfiles.Length > 1)
                    {
                        Console.WriteLine($"{file.Name} ({file.Length / 1024 / 1024}mb):");
                        for (var i = 0; i < foundfiles.Length; i++)
                        {
                            var foundfile = foundfiles[i];
                            foundfile.Found = true;
                            Console.WriteLine($"{i + 1}: " + foundfile.FullName);
                        }
                        Console.Write("Keuze (niets / foute invoer is overslaan): ");
                        var strNumber = Console.ReadLine();
                        if (int.TryParse(strNumber, out int number))
                        {
                            Console.WriteLine($"");
                            for (var i = 0; i < foundfiles.Length; i++)
                            {
                                if (i != number - 1)
                                {
                                    var foundfile = foundfiles[i];

                                    var oldpath = foundfile.FullName;
                                    var newpath = dirmoved + foundfile.FullName.Substring(dir.Length);
                                    var newpathinfo = new FileInfo(newpath);

                                    if (!newpathinfo.Directory.Exists)
                                        newpathinfo.Directory.Create();

                                    File.Move(oldpath, newpath);
                                    Console.WriteLine($"{oldpath} verplaatst naar {newpath}");
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Overslaan...");
                        }
                        Console.WriteLine($"");
                    }
                }
            }
            Console.WriteLine($"Klaar, doei!");
        }
        private static void GetFiles(List<FileItem> files, DirectoryInfo dirinfo)
        {
            var dirfiles = dirinfo.GetFiles();
            foreach (var dirfile in dirfiles)
            {
                if (dirfile.Extension.ToUpper() != ".DRX" &&
                    dirfile.Extension.ToUpper() != ".PRPROJ" &&
                    dirfile.Extension.ToUpper() != ".TXT" &&
                    dirfile.Extension.ToUpper() != ".INI" &&
                    dirfile.Extension.ToUpper() != ".EDL" &&
                    dirfile.Extension.ToUpper() != ".PNG" &&
                    dirfile.Extension.ToUpper() != ".PDV")
                {
                    Console.WriteLine($"Reading #{files.Count}: {dirfile.FullName}");
                    var file = new FileItem(dirfile);
                    files.Add(file);
                }
            }
            foreach (var subdir in dirinfo.GetDirectories())
            {
                GetFiles(files, subdir);
            }
        }

        public class FileItem
        {
            const int SampleSize = 32;

            public FileItem(FileInfo info)
            {
                FullName = info.FullName;
                Name = info.Name;
                Length = info.Length;

                using (var stream = info.OpenRead())
                {
                    if (stream.Length < SampleSize * 3)
                    {
                        // Read all bytes
                        Hash = new byte[stream.Length];
                        stream.Read(Hash, 0, Convert.ToInt32(stream.Length));
                    }
                    else
                    {
                        // Read first, middle and end
                        Hash = new byte[SampleSize * 3];

                        // Read first 16 bytes
                        stream.Read(Hash, 0, SampleSize);

                        // Read middle 16 bytes                       
                        long middleLength = (stream.Length / 2) - (SampleSize / 2);
                        stream.Seek(middleLength, SeekOrigin.Begin);
                        stream.Read(Hash, SampleSize * 1, SampleSize);

                        // Read last 16 bytes
                        long endLength = stream.Length - SampleSize;
                        stream.Seek(endLength, SeekOrigin.Begin);
                        stream.Read(Hash, SampleSize * 2, SampleSize);
                    }
                }
            }

            public string FullName { get; }
            public string Name { get; }
            public long Length { get; }
            public byte[] Hash { get; }

            public bool Found { get; set; }

            public override string ToString()
            {
                return FullName;
            }
        }
    }
}
